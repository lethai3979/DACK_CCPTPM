using AutoMapper;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Service;
using GoWheels_WebAPI.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoWheels_WebAPI.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageUserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly IMapper _mapper;

        public ManageUserController(UserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("GetAllSubmitDriver")]
        public async Task<ActionResult<OperationResult>> GetAllSubmitDriverAsync()
        {
            try
            {
                var users = await _userService.GetAllSubmitDriverAsync();
                var userVMs = _mapper.Map<List<UserVM>>(users);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK,data: userVMs);
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
