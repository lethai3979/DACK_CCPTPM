using AutoMapper;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Service;
using GoWheels_WebAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Controllers
{
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class AmenityController : ControllerBase
    {
        private readonly AmenityService _amenityService;
        private readonly IMapper _mapper;

        public AmenityController(AmenityService amenityService, IMapper mapper)
        {
            _amenityService = amenityService;
            _mapper = mapper;
        }
        [HttpGet("GetAll")]
        public async Task<ActionResult<OperationResult>> GetAllAsync()
        {
            try
            {
                var result = await _amenityService.GetAllAsync();
                var amenityVMs = _mapper.Map<List<AmenityVM>>(result);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: amenityVMs);
            }
            catch(NullReferenceException aEx)
            {
                return new OperationResult(false, aEx.Message, StatusCodes.Status204NoContent);
            }
            catch(AutoMapperMappingException mapperEx)
            {
                return new OperationResult(false, mapperEx.Message, StatusCodes.Status422UnprocessableEntity);
            }
            catch (Exception ex)
            {
                var exMessage = ex.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }

        }

        [HttpGet("GetByIdAsync/{id}")]
        public async Task<ActionResult<OperationResult>> GetById(int id)
        {
            try
            {
                var result = await _amenityService.GetByIdAsync(id);
                var amenityVM = _mapper.Map<AmenityVM>(result);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: amenityVM);
            }
            catch (NullReferenceException aEx)
            {
                return new OperationResult(false, aEx.Message, StatusCodes.Status204NoContent);
            }
            catch (AutoMapperMappingException mapperEx)
            {
                return new OperationResult(false, mapperEx.Message, StatusCodes.Status422UnprocessableEntity);
            }
            catch (Exception ex)
            {
                var exMessage = ex.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Add")]
        public async Task<ActionResult<OperationResult>> AddAsync([FromForm] AmenityDTO amenityDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var amenity = _mapper.Map<Amenity>(amenityDTO);
                    await _amenityService.AddAsync(amenity,amenityDTO.IconImage);
                    return new OperationResult(true, "Amenity add succesfully", StatusCodes.Status200OK);
                }
                return BadRequest("Amenity data invalid");
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

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<OperationResult>> DeleteAsync(int id)
        {
            try
            {
                await _amenityService.DeletedByIdAsync(id);
                return new OperationResult(true, "Amenity deleted succesfully", StatusCodes.Status200OK);
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

        [HttpPut("Update/{id}")]
        public async Task<ActionResult<OperationResult>> UpdateAsync(int id, [FromForm] AmenityDTO amenityDTO)
        {
            try
            {
                if (amenityDTO == null || id != amenityDTO.Id)
                {
                    return BadRequest("Invalid request");
                }
                if (ModelState.IsValid)
                {
                    var amenity = _mapper.Map<Amenity>(amenityDTO);
                    await _amenityService.UpdateAsync(id, amenity, amenityDTO.IconImage);
                    return new OperationResult(true, "Amenity update succesfully", StatusCodes.Status200OK);
                }
                return BadRequest("Amenity data invalid");
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
