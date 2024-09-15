using GoWheels_WebAPI.Models.DTOs;
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

        [HttpGet("GetAllCarType")]
        public async Task<ActionResult<OperationResult>> GetAllAsync()
        {
            var result = await _carTypeService.GetAllAsync();
            return result;
        }
        [HttpPost("AddCarType")]
        public async Task<ActionResult<OperationResult>> AddAsync([FromBody] CarTypeDTO carTypeDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _carTypeService.AddAsync(carTypeDTO);
                return result;
            }
            return BadRequest("Car type data invalid");
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<OperationResult>> DeleteAsync(int id)
        {
            var result = await _carTypeService.DeletedByIdAsync(id);
            return result;
        }

    }
}
