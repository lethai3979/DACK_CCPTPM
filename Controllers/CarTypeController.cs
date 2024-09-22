using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Service;
using GoWheels_WebAPI.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GoWheels_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarTypeController : ControllerBase
    {
        private readonly CarTypeService _carTypeService;

        public CarTypeController(CarTypeService carTypeService)
        {
            _carTypeService = carTypeService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<OperationResult>> GetAllAsync()
            => await _carTypeService.GetAllAsync();

        [HttpPost("Add")]
        public async Task<ActionResult<OperationResult>> AddAsync([FromBody] CarTypeDTO carTypeDTO)
        {
            if (carTypeDTO == null)
            {
                return BadRequest("Car type value is null");
            }
            if (ModelState.IsValid)
            {
                var result = await _carTypeService.AddAsync(carTypeDTO);
                return result;
            }
            return BadRequest("Car type data invalid");
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<OperationResult>> DeleteAsync(int id)
            => await _carTypeService.DeletedByIdAsync(id);
        

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<OperationResult>> GetByIdAsync(int id)
            => await _carTypeService.GetByIdAsync(id);

        [HttpPost("Update/{id}")]
        public async Task<ActionResult<OperationResult>> UpdateAsync(int id, CarTypeDTO carTypeDTO)
        {
            if(carTypeDTO == null || id != carTypeDTO.Id)
            {
                return BadRequest("Invalid request");
            }
            if (ModelState.IsValid)
            {
                var result = await _carTypeService.UpdateAsync(id, carTypeDTO);
                return result;
            }
            return BadRequest("Car type data invalid");
        }
    }
}
