using AutoMapper;
using Azure.Core;
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
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class ReportTypeController : ControllerBase
    {
        private readonly ReportTypeService _reportTypeService;
        private readonly IMapper _mapper;

        public ReportTypeController(ReportTypeService reportTypeService, IMapper mapper)
        {
            _reportTypeService = reportTypeService;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<OperationResult>> GetAllAsync()
        {
            try
            {
                var reportTypes = await _reportTypeService.GetAllAsync();
                var reportTypeVMs = _mapper.Map<List<ReportTypeVM>>(reportTypes);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: reportTypeVMs);
            }
            catch (NullReferenceException nullEx)
            {
                return new OperationResult(false, nullEx.Message, StatusCodes.Status204NoContent);
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


        [HttpGet("GetByIdAsync/{id}")]
        public async Task<ActionResult<OperationResult>> GetByIdAsync(int id)
        {
            try
            {
                var reportType = await _reportTypeService.GetByIdAsync(id);
                var reportTypeVM = _mapper.Map<ReportTypeVM>(reportType);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: reportTypeVM);
            }
            catch (NullReferenceException nullEx)
            {
                return new OperationResult(false, nullEx.Message, StatusCodes.Status204NoContent);
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
        public async Task<ActionResult<OperationResult>> AddAsync(ReportTypeDTO reportTypeDTO)
        {
            try
            {
                if (reportTypeDTO == null)
                {
                    return BadRequest("Report type is null");
                }
                if (ModelState.IsValid)
                {
                    var reportType = _mapper.Map<ReportType>(reportTypeDTO);
                    await _reportTypeService.AddAsync(reportType);
                    return new OperationResult(true, "Report type add succesfully", StatusCodes.Status200OK);

                }
                return BadRequest("Report type data invalid");
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
        public async  Task<ActionResult<OperationResult>> UpdateAsync (int id, ReportTypeDTO reportTypeDTO)
        {
            try
            {
                if (reportTypeDTO == null || id != reportTypeDTO.Id)
                {
                    return BadRequest("Invalid request");
                }
                if (ModelState.IsValid)
                {
                    var reportType = _mapper.Map<ReportType>(reportTypeDTO);
                    await _reportTypeService.UpdateAsync(id, reportType);
                    return new OperationResult(true, "Report type update succesfully", StatusCodes.Status200OK);

                }
                return BadRequest("Report type data invalid");
            }
            catch (NullReferenceException nullEx)
            {
                return new OperationResult(false, nullEx.Message, StatusCodes.Status204NoContent);
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

        [HttpPost("Delete/{id}")]
        public async Task<ActionResult<OperationResult>> DeletedAsync(int id)
        {
            try
            {
                await _reportTypeService.DeletedByIdAsync(id);
                return new OperationResult(true, "Report type deleted succesfully", StatusCodes.Status200OK);
            }
            catch (NullReferenceException nullEx)
            {
                return new OperationResult(false, nullEx.Message, StatusCodes.Status204NoContent);
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
