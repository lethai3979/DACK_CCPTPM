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

        [HttpGet("GetAll")]
        public async Task<ActionResult<OperationResult>> GetAllBookings()
            => await _bookingService.GetAllAsync();

        [HttpGet("GetAllCancelRequestBooking")]
        public async Task<ActionResult<OperationResult>> GetAllCancelRequestBooking()
            => await _bookingService.GetAllCancelRequestAsync();

        [HttpPost("AddAsync")]
        public async Task<ActionResult<OperationResult>> AddAsync(BookingDTO bookingDTO)
            => await _bookingService.AddAsync(bookingDTO);



    }
}
