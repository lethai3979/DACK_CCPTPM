using AutoMapper;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Service;
using GoWheels_WebAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Controllers.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverBookingController : ControllerBase
    {
        private readonly DriverBookingService _driverBookingService;
        private readonly BookingService _bookingService;
        private readonly InvoiceService _invoiceService;
        private readonly IMapper _mapper;

        public DriverBookingController(DriverBookingService driverBookingService, 
                                        BookingService bookingService,
                                        InvoiceService invoiceService, 
                                        IMapper mapper)
        {
            _driverBookingService = driverBookingService;
            _bookingService = bookingService;
            _invoiceService = invoiceService;
            _mapper = mapper;
        }

        [HttpGet("GetAllDriverBooking")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<OperationResult>> GetAllDriverBookingsByUserIdAsync()
        {
            try
            {
                var driverBookings = await _driverBookingService.GetAllByUserIdAsync();
                var driverBookingsVMs = _mapper.Map<List<DriverBookingVM>>(driverBookings);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: driverBookingsVMs);
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

        [HttpPost("AddDriverBooking/{driverBookingId}")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<OperationResult>> AddDriverBookingAsync(int bookingId)
        {
            try
            {
                var booking = await _bookingService.GetByIdAsync(bookingId);
                await _driverBookingService.AddDriverBookingAsync(booking);
                return new OperationResult(true, "Accept booking succesfully", StatusCodes.Status200OK);
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

        [HttpPost("CancelDriverBooking/{driverBookingId}")]
        public async Task<ActionResult<OperationResult>> CancelDriverBookingAsync(int driverBookingId)
        {
            try
            {

                await _driverBookingService.CancelDriverBookingAsync(driverBookingId);
                var invoice = await _invoiceService.GetByDriverBookingIdAsync(driverBookingId);
                invoice.DriverBookingId = null;
                await _invoiceService.UpdateAsync(invoice);
                var booking = await _bookingService.GetByIdAsync(invoice.BookingId);
                booking.HasDriver = false;
                booking.IsRequireDriver = true;
                await _bookingService.UpdateAsync(booking.Id, booking);
                return new OperationResult(true, "Cancel driver booking succesfully", StatusCodes.Status200OK);
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
    }
}
