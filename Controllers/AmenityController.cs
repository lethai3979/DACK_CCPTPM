using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Service;
using GoWheels_WebAPI.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoWheels_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmenityController : ControllerBase
    {
        private readonly AmenityService _amenityService;

        public AmenityController(AmenityService amenityService  )
        {
            _amenityService = amenityService;
        }
        [HttpGet("GetAllAmenity")]
        public async Task<ActionResult<OperationResult>> GetAllAsync()
        {
            var result = await _amenityService.GetAllAsync();
            return result;
        }
        [HttpPost("AddAmenity")]
        public async Task<ActionResult<OperationResult>> AddAsync([FromBody] AmenityDTO amenityDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _amenityService.AddAsync(amenityDTO);
                return result;
            }
            return BadRequest("Amenity data invalid");
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<OperationResult>> DeleteAsync(int id)
        {
            var result = await _amenityService.DeletedByIdAsync(id);
            return result;
        }
        //[HttpPut("Update/{id}")]
        //public async Task<ActionResult<OperationResult>> UpdateAsync(Amenity entity, Amenity newEntity)
        //{
        //    var result = await _amenityService.UpdateAsync(newEntity,entity);
        //    return result;
        //}

    }
}
