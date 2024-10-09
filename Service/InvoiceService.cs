using AutoMapper;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Repositories;
using GoWheels_WebAPI.Models.Entities;
using System.Security.Claims;
using GoWheels_WebAPI.Utilities;
using GoWheels_WebAPI.Payment;
using Newtonsoft.Json.Linq;
using GoWheels_WebAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Service
{
    public class InvoiceService
    {
        private readonly InvoiceRepository _invoiceRepository;
        private readonly BookingService _bookingService;
        private readonly PostService _postService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _userId;
        private readonly IConfiguration _configuration;

        public InvoiceService(InvoiceRepository invoiceRepository,
                                BookingService bookingService,
                                PostService postService,
                                IMapper mapper,
                                IHttpContextAccessor httpContextAccessor,
                                IConfiguration configuration)
        {
            _invoiceRepository = invoiceRepository;
            _bookingService = bookingService;
            _postService = postService;
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

        public async Task<OperationResult> GetByBookingIdAsync(int id)
        {
            var invoice = await _invoiceRepository.GetByBookingIdAsync(id);
            if (invoice == null)
            {
                return new OperationResult(false, message: "Invoice not found", statusCode: StatusCodes.Status204NoContent);
            }
            var invoiceVN = _mapper.Map<InvoiceVM>(invoice);
            return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: invoiceVN);
        }

        public async Task<OperationResult> GetPersonalInvoicesAsync()
        {
            var invoices = await _invoiceRepository.GetAllByUserIdAsync(_userId);
            if (invoices.Count == 0)
            {
                return new OperationResult(false, message: "List empty", statusCode: StatusCodes.Status204NoContent);
            }
            var invoiceVNs = _mapper.Map<List<InvoiceVM>>(invoices);
            return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: invoiceVNs);
        }

        public async Task<OperationResult> GetAllRefundInvoicesAsync()
        {
            var invoices = await _invoiceRepository.GetAllRefundInvoicesAsync();
            if (invoices.Count == 0)
            {
                return new OperationResult(false, message: "List empty", statusCode: StatusCodes.Status204NoContent);
            }
            var invoiceVNs = _mapper.Map<List<InvoiceVM>>(invoices);
            return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: invoiceVNs);
        }

        public async Task<OperationResult> RefundAsync(int bookingId, bool isAccept)
        {
            try
            {
                var bookingVM = await _bookingService.GetByIdAsync(bookingId);
                if (!bookingVM.Success)
                {
                    throw new Exception(bookingVM.Message);
                }
                var booking = _mapper.Map<Booking>((BookingVM)bookingVM.Data!);
                //await _postService.UpdateRideNumberAndIsAvailableAsync(booking.PostId, true, -1);
                await _bookingService.ExamineCancelBookingRequestAsync(booking, isAccept);
                if (isAccept)
                {
                    var invoice = await _invoiceRepository.GetByBookingIdAsync(bookingId) ?? throw new Exception("Invoice not found");
                    var refundValue = (double)invoice!.Total;
                    var refundHours = (booking.RecieveOn - DateTime.Now).TotalHours;
                    var createHours = (booking.CreatedOn - DateTime.Now).TotalHours;
                    if (refundHours > 168 && createHours > 1)
                    {
                        refundValue = refundValue * 0.7;
                    }
                    else if (refundHours <= 168 && createHours > 1)
                    {
                        refundValue = 0;
                    }
                    else if (createHours == 1)
                    {
                        refundValue = (double)invoice.Total;
                    }
                    var refundInvoice = new Invoice()
                    {
                        Total = -(decimal)refundValue,
                        ReturnOn = DateTime.Now,
                        CreatedOn = DateTime.Now,
                        CreatedById = _userId,
                        BookingId = booking.Id,
                    };
                    await _invoiceRepository.AddAsync(refundInvoice);
                }
                return new OperationResult(true, "Cancellation request processed successfully", StatusCodes.Status200OK);
            }
            catch (DbUpdateException dbEx)
            {
                var dbExMessage = dbEx.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, dbExMessage, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                var exMessage = ex.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }

        }

        public async Task<OperationResult> RefundReportedPostBooking(int postId)
        {
            try
            {
                var waitingBookings = await _bookingService.GetAllWaitingBookingAsync(postId);
                if (waitingBookings.Count == 0)
                {
                    return new OperationResult(true, "No booking to refund", StatusCodes.Status200OK);
                }
                foreach (var booking in waitingBookings)
                {
                    var refundInvoice = new Invoice()
                    {
                        Total = -booking.PrePayment,
                        ReturnOn = DateTime.Now,
                        CreatedOn = DateTime.Now,
                        CreatedById = _userId,
                        BookingId = booking.Id,
                    };
                    await _invoiceRepository.AddAsync(refundInvoice);
                }
                return new OperationResult(true, "Refund successfully", StatusCodes.Status200OK);
            }
            catch (DbUpdateException dbEx)
            {
                var dbExMessage = dbEx.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, dbExMessage, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                var exMessage = ex.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }

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

            string orderInfo = _httpContextAccessor.HttpContext?.User?
                     .FindFirstValue(ClaimTypes.Name) ?? "UnknownUser" + " chuyển tiền";
            string amount = priceStr;
            string orderid = DateTime.Now.Ticks.ToString(); // Order ID
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = newBookingIdStr;
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
            var jsonResponse = await PaymentRequest.SendPaymentRequestAsync(endpoint, message.ToString());
            return jsonResponse;
        }

        public async Task<OperationResult> ProcessReturnUrlAsync(IQueryCollection queryParams)
        {
            try
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

                string bookingIdStr = queryParams["extraData"].ToString() ?? "";
                if (!int.TryParse(bookingIdStr, out int bookingIdNum))
                {
                    return new OperationResult(false, message: "Invalid booking ID", statusCode: StatusCodes.Status400BadRequest);
                }

                var bookingResult = await _bookingService.GetByIdAsync(bookingIdNum);

                if (bookingResult.Data == null)
                {
                    return new OperationResult(false, message: "Booking not found", statusCode: StatusCodes.Status404NotFound);
                }
                var booking = _mapper.Map<Booking>((BookingVM)bookingResult.Data);
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
                    await _bookingService.DeleteAsync(booking.Id);
                    await _postService.UpdateRideNumberAndIsAvailableAsync(booking.PostId, true, -1);
                    return new OperationResult(false, message: "Payment failed", statusCode: StatusCodes.Status400BadRequest);
                }
                booking.IsPay = true;
                var bookingDTO = _mapper.Map<BookingDTO>(booking);
                var updateBookingResult = await _bookingService.UpdateAsync(booking.Id, bookingDTO);
                if (!updateBookingResult.Success)
                {
                    return updateBookingResult;
                }
                var invoice = new Invoice()
                {
                    Total = booking.PrePayment,
                    ReturnOn = booking.RecieveOn.AddDays(2),
                    BookingId = booking.Id,
                    CreatedById = booking.UserId,
                    CreatedOn = DateTime.Now
                };
                await _invoiceRepository.AddAsync(invoice);
                bool isAvailable = booking.RecieveOn > DateTime.Now;
                await _postService.UpdateRideNumberAndIsAvailableAsync(booking.PostId, isAvailable, 1);

                return new OperationResult(true, message: "Payment successful", statusCode: StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException?.Message);
            }
        }

    }
}
