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
using System.Globalization;

namespace GoWheels_WebAPI.Controllers.Customer
{
    [Area("User")]
    [Route("api/[area]/[controller]")]
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
        [Authorize(Roles = "User")]
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<OperationResult>> GetById(int id)
        {
            try
            {
                var booking = await _bookingService.GetByIdAsync(id);
                var bookingVM = _mapper.Map<BookingVM>(booking);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: bookingVM);
            }
            catch (NullReferenceException nullEx)
            {
                return new OperationResult(false, nullEx.Message, StatusCodes.Status204NoContent);
            }
            catch (AutoMapperMappingException mapperEx)
            {
                return new OperationResult(false, mapperEx.Message, StatusCodes.Status422UnprocessableEntity);
            }
            catch (Exception ex)
            {
                var exMessage = ex.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }
        public class SumHoursRequest
        {
            public string startdate { get; set; }
            public string enddate { get; set; }
        }

        [HttpPost("Sumhours")]
        public async Task<ActionResult<OperationResult>> SumHours([FromBody] SumHoursRequest request)
            //public async Task<ActionResult<OperationResult>> SumHours(DateTime startdate, DateTime enddate)
        {
            if (request == null || string.IsNullOrEmpty(request.startdate) || string.IsNullOrEmpty(request.enddate))
            {
                return BadRequest("Start date and end date must be provided");
            }

            try
            {
                DateTime startdate = DateTime.ParseExact(request.startdate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                DateTime enddate = DateTime.ParseExact(request.enddate, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                var hours = await _bookingService.GetSumHours(startdate, enddate);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: hours);
            }
            catch (NullReferenceException nullEx)
            {
                return new OperationResult(false, nullEx.Message, StatusCodes.Status204NoContent);
            }
            catch (AutoMapperMappingException mapperEx)
            {
                return new OperationResult(false, mapperEx.Message, StatusCodes.Status422UnprocessableEntity);
            }
            catch (Exception ex)
            {
                var exMessage = ex.Message ?? "An error the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }
        [Authorize(Roles = "User")]
        [HttpGet("GetPersonalBookings")]
        public async Task<ActionResult<OperationResult>> GetPersonalBookings()
        {
            try
            {
                var bookings = await _bookingService.GetPersonalBookingsAsync();
                var bookingVMs = _mapper.Map<List<BookingVM>>(bookings);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: bookingVMs);
            }
            catch (NullReferenceException nullEx)
            {
                return new OperationResult(false, nullEx.Message, StatusCodes.Status204NoContent);
            }
            catch (AutoMapperMappingException mapperEx)
            {
                return new OperationResult(false, mapperEx.Message, StatusCodes.Status422UnprocessableEntity);
            }
            catch (Exception ex)
            {
                var exMessage = ex.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }


        }
        [Authorize(Roles = "User")]
        [HttpGet("GetAllPendingBookingsByUserId")]
        public async Task<ActionResult<OperationResult>> GetAllPendingBookingsByUserId()
        {
            try
            {
                var bookings = await _bookingService.GetPendingBookingsByUserIdAsync();
                var bookingVMs = _mapper.Map<List<BookingVM>>(bookings);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: bookingVMs);
            }
            catch (NullReferenceException nullEx)
            {
                return new OperationResult(false, nullEx.Message, StatusCodes.Status204NoContent);
            }
            catch (AutoMapperMappingException mapperEx)
            {
                return new OperationResult(false, mapperEx.Message, StatusCodes.Status422UnprocessableEntity);
            }
            catch (Exception ex)
            {
                var exMessage = ex.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }
        [Authorize(Roles = "User")]
        [HttpPost("SendCancelRequest/{id}")]
        public async Task<ActionResult<OperationResult>> SendCancelRequestBookingAsync(int id)
        {
            try
            {
                var existingInvoice = await _invoiceService.GetByBookingIdAsync(id);
                if (existingInvoice != null)
                {
                    await _bookingService.RequestCancelBookingAsync(id);
                }
                return new OperationResult(true, "Cancellation request sent succesfully", StatusCodes.Status200OK);
            }
            catch (NullReferenceException nullEx)
            {
                return new OperationResult(false, nullEx.Message, StatusCodes.Status204NoContent);
            }
            catch (DbUpdateException dbEx)
            {
                return new OperationResult(false, dbEx.Message, StatusCodes.Status500InternalServerError);
            }
            catch (InvalidOperationException operationEx)
            {
                return new OperationResult(false, operationEx.Message, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, StatusCodes.Status400BadRequest);
            }

        }

        [Authorize(Roles = "User")]
        [HttpPost("Add")]
        public async Task<ActionResult<OperationResult>> AddAsync(BookingDTO bookingDTO)
        {
            try
            {
/*                if (bookingDTO.RecieveOn < DateTime.Now || bookingDTO.ReturnOn < bookingDTO.RecieveOn)
                {
                    return BadRequest("return date or recieve date invalid");
                }*/
                if (ModelState.IsValid)
                {
                    var booking = _mapper.Map<Booking>(bookingDTO);
                    await _bookingService.AddAsync(booking);
                    return new OperationResult(true, "Booking add succesfully", StatusCodes.Status200OK);
                }
                return BadRequest("Booking data invalid");
            }
            catch (DbUpdateException dbEx)
            {
                return new OperationResult(false, dbEx.Message, StatusCodes.Status500InternalServerError);
            }
            catch (InvalidOperationException operationEx)
            {
                return new OperationResult(false, operationEx.Message, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, StatusCodes.Status400BadRequest);
            }
        }
        [Authorize(Roles = "User")]
        [HttpPut("UpdateOwnerConfirm")]
        public async Task<ActionResult<OperationResult>> UpdateOwnerConfirmAsync(BookingDTO bookingDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var booking = _mapper.Map<Booking>(bookingDTO);
                    await _bookingService.AddAsync(booking);
                    return new OperationResult(true, "Booking add succesfully", StatusCodes.Status200OK);
                }
                return BadRequest("Booking data invalid");
            }
            catch (DbUpdateException dbEx)
            {
                return new OperationResult(false, dbEx.Message, StatusCodes.Status500InternalServerError);
            }
            catch (InvalidOperationException operationEx)
            {
                return new OperationResult(false, operationEx.Message, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, StatusCodes.Status400BadRequest);
            }
        }

    }
}
