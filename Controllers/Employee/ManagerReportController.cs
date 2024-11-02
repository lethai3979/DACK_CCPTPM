using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Service;
using GoWheels_WebAPI.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManagerReportController : ControllerBase
    {
        private readonly ReportService _reportService;

        public ManagerReportController(ReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPut("ExamineReport/{id}")]
        public async Task<ActionResult<OperationResult>> ExamineReportPost(int id, bool isAccept)
        {
            try
            {
                await _reportService.ConfirmReportAsync(id, isAccept);
                return new OperationResult(true, "Report handled succesfully", StatusCodes.Status200OK);
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
