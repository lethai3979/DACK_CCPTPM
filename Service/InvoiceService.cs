using AutoMapper;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Repositories;
using GoWheels_WebAPI.Models.Entities;
using System.Security.Claims;
using GoWheels_WebAPI.Utilities;
using GoWheels_WebAPI.Payment;
using Newtonsoft.Json.Linq;

namespace GoWheels_WebAPI.Service
{
    public class InvoiceService
    {
        private readonly InvoiceRepository _invoiceRepository;
        private readonly BookingRepository _bookingRepository;
        private readonly PostRepository _postRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _userId;
        private readonly IConfiguration _configuration;

        public InvoiceService(InvoiceRepository invoiceRepository,
                                BookingRepository bookingRepository,
                                PostRepository postRepository,
                                IMapper mapper,
                                IHttpContextAccessor httpContextAccessor,
                                IConfiguration configuration)
        {
            _invoiceRepository = invoiceRepository;
            _bookingRepository = bookingRepository;
            _postRepository = postRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor.HttpContext?.User?
                     .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
            _configuration = configuration;
        }

        public async Task<OperationResult> GetAllAsync()
        {
            var invoices = await _invoiceRepository.GetAllAsync();
            if (invoices.Count == 0)
            {
                return new OperationResult(false, message: "List empty", statusCode: StatusCodes.Status204NoContent);
            }
            var invoiceVNs = _mapper.Map<InvoiceVM>(invoices);
            return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: invoiceVNs);
        }
        public async Task<OperationResult> GetByIdAsync(int id)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(id);
            if (invoice == null)
            {
                return new OperationResult(false, message: "Invoice not found", statusCode: StatusCodes.Status204NoContent);
            }
            var invoiceVN = _mapper.Map<InvoiceVM>(invoice);
            return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: invoiceVN);
        }

        public async Task<OperationResult> GetPersonalInvoices()
        {
            var invoices = await _invoiceRepository.GetAllByUserIdAsync(_userId);
            if (invoices.Count == 0)
            {
                return new OperationResult(false, message: "List empty", statusCode: StatusCodes.Status204NoContent);
            }
            var invoiceVNs = _mapper.Map<InvoiceVM>(invoices);
            return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: invoiceVNs);
        }

        public async Task<string> ProcessMomoPaymentAsync(Booking  booking)
        {
            double price = (double)booking.PrePayment;
            string priceStr = price.ToString();
            string newBookingIdStr = booking.Id.ToString();

            string endpoint = _configuration.GetValue<string>("MomoAPI:MomoApiUrl") ?? string.Empty;
            string serectkey = _configuration.GetValue<string>("MomoAPI:Serectkey") ?? string.Empty;
            string accessKey = _configuration.GetValue<string>("MomoAPI:AccessKey") ?? string.Empty;
            string returnUrl = _configuration.GetValue<string>("MomoAPI:ReturnUrl") ?? string.Empty;
            string notifyUrl = _configuration.GetValue<string>("MomoAPI:NotifyUrl") ?? string.Empty;
            string partnerCode = _configuration.GetValue<string>("MomoAPI:PartnerCode") ?? string.Empty;

            string orderInfo = newBookingIdStr;
            string amount = priceStr;
            string orderid = DateTime.Now.Ticks.ToString(); // Order ID
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";
            // Create the signature
            string rawHash = $"partnerCode={partnerCode}&accessKey={accessKey}&requestId={requestId}&amount={amount}&orderId={orderid}&orderInfo={orderInfo}&returnUrl={returnUrl}&notifyUrl={notifyUrl}&extraData={extraData}";

            MomoSecurity crypto = new();
            string signature = crypto.signSHA256(rawHash, serectkey);

            // Build request JSON
            JObject message = new()
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyUrl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }
            };

            // Send payment request through the repository
            var jsonResponse =  PaymentRequest.sendPaymentRequest(endpoint, message.ToString());
            return jsonResponse;
        }

        public async Task<OperationResult> ProcessReturnUrlAsync(IQueryCollection queryParams)
        {
            string param = "";
            param += "partnerCode=" + queryParams["partnerCode"];
            param += "&accessKey=" + queryParams["accessKey"];
            param += "&requestId=" + queryParams["requestId"];
            param += "&amount=" + queryParams["amount"];
            param += "&orderId=" + queryParams["orderId"];
            param += "&orderInfo=" + queryParams["orderInfo"];
            param += "&orderType=" + queryParams["orderType"];
            param += "&transId=" + queryParams["transId"];
            param += "&message=" + queryParams["message"];
            param += "&localMessage=" + queryParams["localMessage"];
            param += "&responseTime=" + queryParams["responseTime"];
            param += "&errorCode=" + queryParams["errorCode"];
            param += "&payType=" + queryParams["payType"];
            param += "&extraData=" + queryParams["extraData"];

            string bookingIdStr = queryParams["orderInfo"].ToString() ?? "";
            if (!int.TryParse(bookingIdStr, out int bookingIdNum))
            {
                return new OperationResult(false, message: "Invalid booking ID", statusCode: StatusCodes.Status400BadRequest);
            }

            var booking = await _bookingRepository.GetByIdAsync(bookingIdNum);
            if (booking == null)
            {
                return new OperationResult(false, message: "Booking not found", statusCode: StatusCodes.Status404NotFound);
            }

            MomoSecurity crypto = new();
            string serectkey = _configuration.GetValue<string>("MomoAPI:Serectkey") ?? "";
            string signature = crypto.signSHA256(param, serectkey);

            if (signature != queryParams["signature"].ToString())
            {
                return new OperationResult(false, message: "Invalid request signature", statusCode: StatusCodes.Status400BadRequest);
            }

            if (queryParams["errorCode"] != "0" || queryParams["errorCode"] == "1006" || queryParams["message"].ToString().Contains("Transaction denied by user"))
            {
                booking.IsDeleted = true;
                await _bookingRepository.UpdateAsync(booking);
                await UpdatePostInfoAsync(booking, true, -1);
                return new OperationResult(false, message: "Payment failed", statusCode: StatusCodes.Status400BadRequest); 
            }
            booking.IsPay = true;

            var invoice = new Invoice()
            {
                Total = booking.PrePayment,
                ReturnOn = booking.RecieveOn.AddDays(2),
                BookingId = booking.Id,
                CreatedById = booking.CreatedById,
                CreatedOn = booking.CreatedOn,
            };

            await _invoiceRepository.AddAsync(invoice);

            bool isAvailable = booking.RecieveOn > DateTime.Now;
            await UpdatePostInfoAsync(booking, isAvailable, 1);

            return new OperationResult(true, message: "Payment successful", statusCode: StatusCodes.Status200OK); 
        }

        private async Task UpdatePostInfoAsync(Booking booking, bool isAvailable, int rideNumber)
        {
            var post = await _postRepository.GetByIdAsync(booking.PostId);
            if (post != null)
            {
                post.IsAvailable = isAvailable;
                if (post.RideNumber == 0 && rideNumber < 0)
                {
                    post.RideNumber = 0;
                }
                else
                {
                    post.RideNumber += rideNumber;
                }
                try
                {
                    await _postRepository.UpdateAsync(post);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.InnerException?.Message);
                }
            }
        }
    }
}
