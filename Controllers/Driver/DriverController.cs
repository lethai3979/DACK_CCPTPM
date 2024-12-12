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
        private readonly IUserService _userService;
        private readonly IHubContext<NotifyHub> _hubContext;
        private readonly IMapper _mapper;

        public DriverController(IDriverService driverService, 
                                IUserService userService, 
                                IHubContext<NotifyHub> hubContext,
                                IMapper mapper)
        {
            _driverService = driverService;
            _userService = userService;
            _hubContext = hubContext;
            _mapper = mapper;
        }

        [HttpGet("GetAllDrivers")]
        public async Task<ActionResult<OperationResult>> GetAllDriversAsync()
        {
            try
            {
                var drivers = await _driverService.GetAll();
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

        [HttpGet("TestConvert")]
        public async Task<ActionResult<OperationResult>> TestConvert()
        {
            try
            {
                await _hubContext.Clients.Group("group1").SendAsync("RecieveMessage", "hi");
                return new OperationResult(true, statusCode: StatusCodes.Status200OK);
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



    }
}
