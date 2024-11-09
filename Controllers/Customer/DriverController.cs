using AutoMapper;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Service;
using GoWheels_WebAPI.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoWheels_WebAPI.Controllers.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly DriverService _driverService;
        private readonly IMapper _mapper;

        public DriverController(DriverService driverService, IMapper mapper)
        {
            _driverService = driverService;
            _mapper = mapper;
        }

        [HttpGet("GetAllDrivers")]
        public async Task<ActionResult<OperationResult>> GetAllDriversAsync()
        {
            try
            {
                var drivers = await _driverService.GetAllAsync();
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
    }
}
