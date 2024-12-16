using AutoMapper;
using Azure.Core;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Service;
using GoWheels_WebAPI.Service.Interface;
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
    public class ReportTypeController : ControllerBase
    {
        private readonly IReportTypeService _reportTypeService;
        private readonly IMapper _mapper;

        public ReportTypeController(IReportTypeService reportTypeService, IMapper mapper)
        {
            _reportTypeService = reportTypeService;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public ActionResult<OperationResult> GetAll()
        {
            try
            {
                var reportTypes = _reportTypeService.GetAll();
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


        [HttpGet("GetById/{id}")]
        public ActionResult<OperationResult> GetById(int id)
        {
            try
            {
                var reportType = _reportTypeService.GetById(id);
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
        [Authorize(Roles = "Admin")]
        public ActionResult<OperationResult> Add([FromForm] ReportTypeDTO reportTypeDTO)
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
                    _reportTypeService.Add(reportType);
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
        [Authorize(Roles = "Admin")]
        public ActionResult<OperationResult> Update(int id, [FromForm] ReportTypeDTO reportTypeDTO)
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
                    _reportTypeService.Update(id, reportType);
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
        [Authorize(Roles = "Admin")]
        public ActionResult<OperationResult> Deleted(int id)
        {
            try
            {
                _reportTypeService.DeletedById(id);
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
