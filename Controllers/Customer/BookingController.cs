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
