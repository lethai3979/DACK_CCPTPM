using AutoMapper;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Service;
using GoWheels_WebAPI.Service.Interface;
using GoWheels_WebAPI.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GoWheels_WebAPI.Controllers.Customer
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotifiyController : ControllerBase
    {
        private readonly INotifyService _notifyService;
        private readonly IMapper _mapper;
        public NotifiyController(INotifyService notifyService, IMapper mapper)
        {
            _notifyService = notifyService;
            _mapper = mapper;
        }

        [HttpGet("GetAllNotify")]
        public async Task<ActionResult<OperationResult>> GetAllByUserId()
        {
            try
            {
                var notifications = await _notifyService.GetAllByUserIdAsync();
                var notificationVMs = _mapper.Map<List<NotifyVM>>(notifications);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: notificationVMs);
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

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<OperationResult>> GetById(int id)
        {
            try
            {
                var notification = await _notifyService.GetByIdAsync(id);
                var notificationVM = _mapper.Map<NotifyVM>(notification);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: notificationVM);
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

        [HttpPut("MarkAsRead/{id}")]
        public async Task<ActionResult<OperationResult>> MarkAsReadAsync(int id)
        {
            try
            {
                await _notifyService.MarkAsReadAsync(id);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK);
            }
            catch (NullReferenceException nullEx)
            {
                return new OperationResult(false, nullEx.Message, StatusCodes.Status204NoContent);
            }
            catch (DbUpdateException dbEx)
            {
                return new OperationResult(false, dbEx.Message, StatusCodes.Status500InternalServerError);
            }
            catch (UnauthorizedAccessException authEx)
            {
                return new OperationResult(false, authEx.Message, StatusCodes.Status401Unauthorized);
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

        [HttpDelete("DeleteNotify/{id}")]
        public async Task<ActionResult<OperationResult>> DeleteNotify(int id)
        {
            try
            {
                await _notifyService.DeleteAsync(id);
                return new OperationResult(true, "Deleted notify succesfully", StatusCodes.Status200OK);
            }
            catch (NullReferenceException nullEx)
            {
                return new OperationResult(false, nullEx.Message, StatusCodes.Status204NoContent);
            }
            catch (DbUpdateException dbEx)
            {
                return new OperationResult(false, dbEx.Message, StatusCodes.Status500InternalServerError);
            }
            catch (UnauthorizedAccessException authEx)
            {
                return new OperationResult(false, authEx.Message, StatusCodes.Status401Unauthorized);
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
