using AutoMapper;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Service;
using GoWheels_WebAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet("GetAllUser")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult<OperationResult>> GetAllUserAsync()
        {
            try
            {
                var users = await _userService.GetAllUserAsync();
                var userVMs = new List<UserVM>();
                foreach(var user in users) 
                {
                    var userRoles = await _userService.GetUserRolesAsync(user);
                    var userVM = _mapper.Map<UserVM>(user);
                    if(userRoles.Any(r => !r.Equals("Admin")))
                    {
                        userVM.Roles = userRoles;
                        userVMs.Add(userVM);
                    }
                }
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: userVMs);
            }
            catch (AutoMapperMappingException mapperEx)
            {
                return new OperationResult(false, mapperEx.Message, StatusCodes.Status422UnprocessableEntity);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, StatusCodes.Status400BadRequest);
            }
        }

        [HttpGet("GetUserInfo/{userId}")]
        public async Task<ActionResult<OperationResult>> GetByUserIdAsync(string userId)
        {
            try
            {
                var user = await _userService.GetByUserIdAsync(userId);
                var userVM = _mapper.Map<UserVM>(user);
                var userRoles = await _userService.GetUserRolesAsync(user);
                userVM.Roles = userRoles;
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: userVM);
            }
            catch (AutoMapperMappingException mapperEx)
            {
                return new OperationResult(false, mapperEx.Message, StatusCodes.Status422UnprocessableEntity);
            }
            catch (NullReferenceException nullEx)
            {
                return new OperationResult(false, nullEx.Message, StatusCodes.Status204NoContent);

            }
            catch (Exception ex)
            {
                var exMessage = ex.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }

        [HttpGet("GetAllDriverSubmit")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult<OperationResult>> GetAllDriverSubmitAsync()
        {
            try
            {
                var users = await _userService.GetAllDriverSubmitAsync();
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

        [HttpPost("ExamineDriverSubmit")]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<ActionResult<OperationResult>> ExamineDriverSubmit([FromForm] string userId, [FromForm] bool isAccept)
        {
            try
            {
                await _userService.ConfirmDriverSubmit(userId, isAccept);
                return new OperationResult(true, "Driver submit confirmed", StatusCodes.Status200OK);
            }
            catch (NullReferenceException nullEx)
            {
                return new OperationResult(false, nullEx.Message, StatusCodes.Status404NotFound);
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

        [HttpPut("LockUserAccount/{userId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<OperationResult>> LockUserAccount(string userId)
        {
            try
            {
                await _userService.UpdateUserLockAccountAsync(userId);
                return new OperationResult(true,statusCode: StatusCodes.Status200OK);
            }
            catch (NullReferenceException nullEx)
            {
                return new OperationResult(false, nullEx.Message, StatusCodes.Status404NotFound);
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
