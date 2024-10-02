using AutoMapper;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Service;
using GoWheels_WebAPI.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace GoWheels_WebAPI.Controllers.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly BookingService _bookingService;
        private readonly InvoiceService _invoiceService;
        private IMapper _mapper;

        public BookingController(BookingService bookingService, InvoiceService invoiceService, IMapper mapper)
        {
            _bookingService = bookingService;
            _invoiceService = invoiceService;
            _mapper = mapper;
        }

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<OperationResult>> GetById(int id)
            => await _bookingService.GetByIdAsync(id);

        [HttpGet("GetPersonalBookings")]
        public async Task<ActionResult<OperationResult>> GetPersonalBookings()
            => await _bookingService.GetPersonalBookingAsync();

        [HttpGet("GetAll")]
        public async Task<ActionResult<OperationResult>> GetAllBookings()
            => await _bookingService.GetAllAsync();

        [HttpGet("GetAllCancelRequestBooking")]
        public async Task<ActionResult<OperationResult>> GetAllCancelRequestBooking()
            => await _bookingService.GetAllCancelRequestAsync();

        [HttpPost("AddAsync")]
        public async Task<ActionResult<OperationResult>> AddAsync(BookingDTO bookingDTO)
            => await _bookingService.AddAsync(bookingDTO);

        [HttpPost("MomoPayment")]
        public async Task<ActionResult<OperationResult>> MomoPayment(int bookingId)
        {
            var result = await _bookingService.GetByIdAsync(bookingId);
            if(result.Data == null)
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
                    return BadRequest(new { message = "Lỗi khi xử lý URL trả về" });
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
