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
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GoWheels_WebAPI.Controllers
{
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class CarTypeController : ControllerBase
    {
        private readonly CarTypeService _carTypeService;
        private readonly IMapper _mapper;
        

        public CarTypeController(CarTypeService carTypeService, IMapper mapper)
        {
            _carTypeService = carTypeService;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<OperationResult>> GetAllAsync()
        {
            try
            {
                var carTypesList = await _carTypeService.GetAllAsync();
                var carTypeVMs = _mapper.Map<List<CarTypeVM>>(carTypesList);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: carTypeVMs);
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
                var exMessage = ex.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }

        [HttpGet("GetByIdAsync/{id}")]
        public async Task<ActionResult<OperationResult>> GetByIdAsync(int id)
        {
            try
            {
                var carType = await _carTypeService.GetByIdAsync(id);
                var carTypeVM = _mapper.Map<CarTypeVM>(carType);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: carTypeVM);
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
                var exMessage = ex.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }

        [HttpPost("Add")]
        public async Task<ActionResult<OperationResult>> AddAsync([FromBody] CarTypeDTO carTypeDTO)
        {
            try
            {
                if (carTypeDTO == null)
                {
                    return BadRequest("Car type value is null");
                }
                if (ModelState.IsValid)
                {
                    var carType = _mapper.Map<CarType>(carTypeDTO);
                    await _carTypeService.AddAsync(carType, carTypeDTO.CompanyIds);
                    return new OperationResult(true, "Car type add succesfully", StatusCodes.Status200OK);
                }
                return BadRequest("Car type data invalid");
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
                await _carTypeService.DeleteByIdAsync(id);
                return new OperationResult(true, "Car type deleted succesfully", StatusCodes.Status200OK);
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

        



        [HttpPost("Update/{id}")]
        public async Task<ActionResult<OperationResult>> UpdateAsync(int id, CarTypeDTO carTypeDTO)
        {
            try
            {
                if (carTypeDTO == null || id != carTypeDTO.Id)
                {
                    return BadRequest("Invalid request");
                }
                if (ModelState.IsValid)
                {
                    var carType = _mapper.Map<CarType>(carTypeDTO);
                    await _carTypeService.UpdateAsync(id, carType, carTypeDTO.CompanyIds);
                    return new OperationResult(true, "Car type update succesfully", StatusCodes.Status200OK);

                }
                return BadRequest("Car type data invalid");
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
