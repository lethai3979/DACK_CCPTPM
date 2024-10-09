using AutoMapper;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Service;
using GoWheels_WebAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace GoWheels_WebAPI.Controllers.Customer
{
    [Area("User")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
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

        [HttpPost("SendCancelRequest/{id}")]
        public async Task<ActionResult<OperationResult>> SendCancelRequestBookingAsync(int id)
        {
            var isPaidResult = await _invoiceService.GetByBookingIdAsync(id);
            if (isPaidResult.Data != null)
            {
                return await _bookingService.RequestCancelBookingAsync(id);
            }
            return isPaidResult;

        }


        [HttpPost("AddAsync")]
        public async Task<ActionResult<OperationResult>> AddAsync(BookingDTO bookingDTO)
        {
            if(bookingDTO.RecieveOn < DateTime.Now || bookingDTO.ReturnOn < bookingDTO.RecieveOn)
            {
                return BadRequest("return date or recieve date invalid");
            }    
            if(ModelState.IsValid)
            {
                await _bookingService.AddAsync(bookingDTO);
            }
            return BadRequest("Booking data invalid");
        }




    }
}
