using AutoMapper;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Service.Interface;
using GoWheels_WebAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GoWheels_WebAPI.Controllers.Driver
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverBookingController : ControllerBase
    {
        private readonly IDriverBookingService _driverBookingService;
        private readonly IBookingService _bookingService;
        private readonly IInvoiceService _invoiceService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _userId;
        private readonly IMapper _mapper;

        public DriverBookingController(IDriverBookingService driverBookingService,
                                        IBookingService bookingService,
                                        IInvoiceService invoiceService,
                                        IHttpContextAccessor httpContextAccessor,
                                        IMapper mapper)
        {
            _driverBookingService = driverBookingService;
            _bookingService = bookingService;
            _invoiceService = invoiceService;
            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor.HttpContext?.User?
                        .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
            _mapper = mapper;
        }

        [HttpGet("GetAllDriverBookings")]
        [Authorize(Roles = "Driver")]
        public ActionResult<OperationResult> GetAllDriverBookingsByUserId()
        {
            try
            {
                var driverBookings = _driverBookingService.GetAllByUserId();
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

        [HttpPost("AddDriverBooking/{bookingId}")]
        [Authorize(Roles = "Driver")]
        public ActionResult<OperationResult> AddDriverBooking(int bookingId)
        {
            try
            {
                var booking = _bookingService.GetById(bookingId);
                if (booking.HasDriver)
                {
                    return new OperationResult(false, "Driver already assigned", StatusCodes.Status409Conflict);
                }
                _driverBookingService.AddDriverBooking(booking);
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
        [Authorize(Roles = "Driver")]
        public ActionResult<OperationResult> CancelDriverBooking(int driverBookingId) // hủy tài xế cho đơn
        {
            try
            {
                var driverBooking = _driverBookingService.GetById(driverBookingId);
                if (driverBooking.Driver.UserId != _userId)
                {
                    return new OperationResult(false, "Unauthorized", StatusCodes.Status401Unauthorized);
                }
                driverBooking.IsCancel = true;
                _driverBookingService.Update(driverBooking);
                var invoice = _invoiceService.GetByDriverBookingId(driverBookingId);
                _invoiceService.UpdateCancelDriverBooking(invoice, driverBooking.Total);
                var booking = _bookingService.GetById(invoice.BookingId);
                booking.HasDriver = false;
                booking.IsRequireDriver = true;
                _bookingService.Update(booking.Id, booking);
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
