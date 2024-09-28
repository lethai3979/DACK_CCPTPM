using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.ViewModels;
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
    public class CompanyController : ControllerBase
    {
        private readonly CompanyService _companyService;

        public CompanyController(CompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<OperationResult>> GetAllAsync()
            => await _companyService.GetAllAsync();

        [HttpGet("GetByIdAsync/{id}")]
        public async Task<ActionResult<OperationResult>> GetById(int id)
            => await _companyService.GetByIdAsync(id);

        [HttpPost("Add")]
        public async Task<ActionResult<OperationResult>> AddAsync(CompanyDTO companyDTO)
        {
            if (companyDTO == null)
            {
                return BadRequest("Company value is null");
            }
            if (ModelState.IsValid)
            {

                var result = await _companyService.AddAsync(companyDTO);
                return result;
            }
            return BadRequest("Company value invalid");
        }

        [HttpPost("DeleteByIdAsync/{id}")]
        public async Task<ActionResult<OperationResult>> DeleteAsync(int id)
            => await _companyService.DeleteByIdAsync(id);

        [HttpPost("Update/{id}")]
        public async Task<ActionResult<OperationResult>> UpdateAsync(int id, CompanyDTO companyDTO)
        {
            if (companyDTO == null || id != companyDTO.Id)
            {
                return BadRequest("Invalid request");
            }
            if (ModelState.IsValid)
            {

                var result = await _companyService.UpdateAsync(id, companyDTO);
                return result;
            }
            return BadRequest("Company value invalid");

        }
    }
}
