using Azure.Core;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Service;
using GoWheels_WebAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoWheels_WebAPI.Controllers
{
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class ReportTypeController : ControllerBase
    {
        private readonly ReportTypeService _reportTypeService;

        public ReportTypeController(ReportTypeService reportTypeService)
        {
            _reportTypeService = reportTypeService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<OperationResult>> GetAllAsync()
         => await _reportTypeService.GetAllAsync();

        [HttpGet("GetByIdAsync/{id}")]
        public async Task<ActionResult<OperationResult>> GetByIdAsync(int id)
         => await _reportTypeService.GetByIdAsync(id);

        [HttpPost("Add")]
        public async Task<ActionResult<OperationResult>> AddAsync(ReportTypeDTO reportTypeDTO)
        {
            if(reportTypeDTO == null)
            {
                return BadRequest("Report type is null");
            }    
            if(ModelState.IsValid)
            {
                var result = await _reportTypeService.AddAsync(reportTypeDTO);
                return result;
            }
            return BadRequest("Report type data invalid");
        }

        [HttpPost("Update/{id}")]
        public async  Task<ActionResult<OperationResult>> UpdateAsync (int id, ReportTypeDTO reportTypeDTO)
        {
            if (reportTypeDTO == null || id != reportTypeDTO.Id)
            {
                return BadRequest("Invalid request");
            }
            if (ModelState.IsValid)
            {
                var result = await _reportTypeService.UpdateAsync(id, reportTypeDTO);
                return result;
            }
            return BadRequest("Report type data invalid");
        }

        [HttpPost("Delete/{id}")]
        public async Task<ActionResult<OperationResult>> DeletedAsync(int id)
         => await _reportTypeService.DeletedByIdAsync(id);
    }
}
