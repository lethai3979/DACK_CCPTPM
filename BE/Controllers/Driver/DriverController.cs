using AutoMapper;
using GoWheels_WebAPI.Hubs;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Service;
using GoWheels_WebAPI.Service.Interface;
using GoWheels_WebAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace GoWheels_WebAPI.Controllers.Driver
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IDriverService _driverService;
        private readonly IBookingService _bookingService;
        private readonly IInvoiceService _invoiceService;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public DriverController(IDriverService driverService,
                                IBookingService bookingService,
                                IInvoiceService invoiceService,
                                IUserService userService, 
                                IMapper mapper)
        {
            _driverService = driverService;
            _bookingService = bookingService;
            _invoiceService = invoiceService;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("GetAllDrivers")]
        public ActionResult<OperationResult> GetAllDrivers()
        {
            try
            {
                var drivers = _driverService.GetAll();
                var driverVMs = _mapper.Map<List<DriverVM>>(drivers);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: driverVMs);
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

        [HttpGet("UpdateUserLocation/{latitude}&&{longitude}")]
        [Authorize]
        public async Task<ActionResult<OperationResult>> UpdateDriverLocation(string latitude, string longitude)
        {
            try
            {
                await _userService.UpdateDriverLocationAsync(longitude, latitude);
                return new OperationResult(true, "Update location succesfully", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, StatusCodes.Status400BadRequest);
            }
        }

        [HttpPut("AddDriverToBooking/{bookingId}")]
        [Authorize]
        public async Task<ActionResult<OperationResult>> AddDriverToBooking(int bookingId)
        {
            try
            {
                await _bookingService.AddDriverToBookingAsync(bookingId);
                return new OperationResult(true, "Select booking succesfully", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, StatusCodes.Status400BadRequest);
            }
        }

        [HttpPut("RemoveDriverFromBooking/{bookingId}")]
        [Authorize]
        public async Task<ActionResult<OperationResult>> RemoveDriverFromBooking(int bookingId)
        {
            try
            {
                await _bookingService.RemoveDriverFromBookingAsync(bookingId);
                _driverService.UpdateTrustLevel(-1);
                return new OperationResult(true, "Cancel booking succesfully", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, StatusCodes.Status400BadRequest);
            }
        }

    }
}
