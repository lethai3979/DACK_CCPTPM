using AutoMapper;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Service;
using GoWheels_WebAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace GoWheels_WebAPI.Controllers.Customer
{
    [Area("User")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly InvoiceService _invoiceService;
        private readonly BookingService _bookingService;
        private readonly IMapper _mapper;
        public InvoiceController(InvoiceService invoiceService, BookingService bookingService, IMapper mapper)
        {
            _invoiceService = invoiceService;
            _bookingService = bookingService;
            _mapper = mapper;
        }

        [HttpGet("GetPersonalInvoices")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<OperationResult>> GetPersonalInvoicesAsync()
            => await _invoiceService.GetPersonalInvoicesAsync();

        [HttpPost("MomoPayment")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<OperationResult>> MomoPayment(int bookingId)
        {
            var result = await _bookingService.GetByIdAsync(bookingId);
            if (result.Data == null)
            {
                return result;
            }
            Booking booking = _mapper.Map<Booking>((BookingVM)result.Data);
            try
            {
                var responseFromMomo = await _invoiceService.ProcessMomoPaymentAsync(booking);
                JObject jmessage = JObject.Parse(responseFromMomo);
                var payUrlToken = jmessage.GetValue("payUrl");
                if (payUrlToken != null)
                {
                    string payUrl = payUrlToken.ToString();
                    if (!string.IsNullOrEmpty(payUrl))
                    {
                        return Ok(payUrl);
                    }
                }

                return Ok(responseFromMomo); // Handle failure case
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("ReturnUrl")]
        public async Task<ActionResult<OperationResult>> ReturnUrl()
        {
            try
            {
                var result = await _invoiceService.ProcessReturnUrlAsync(Request.Query);

                if (result == null)
                {
                    return BadRequest(new { message = "Error while handling return URL" });
                }

                return Ok(result.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }
    }
}
