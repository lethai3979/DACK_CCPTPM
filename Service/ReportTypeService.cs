using AutoMapper;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Repositories;
using GoWheels_WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GoWheels_WebAPI.Service
{
    public class ReportTypeService
    {
        private readonly ReportTypeRepository _reportTypeRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _userId;
        public ReportTypeService(ReportTypeRepository reportTypeRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _reportTypeRepository = reportTypeRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor.HttpContext?.User?
                     .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
        }

        public async Task<OperationResult> AddAsync(ReportTypeDTO reportTypeDTO)
        {
            try
            {
                var reportType = _mapper.Map<ReportType>(reportTypeDTO);
                reportType.CreatedById = _userId;
                reportType.CreatedOn = DateTime.Now;
                reportType.IsDeleted = false;
                await _reportTypeRepository.AddAsync(reportType);
                return new OperationResult(true, "Report type add succesfully", StatusCodes.Status200OK);
            }

            catch (DbUpdateException dbEx)
            {
                var dbExMessage = dbEx.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, dbExMessage, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                var exMessage = ex.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }

        public async Task<OperationResult> UpdateAsync(int id, ReportTypeDTO reportTypeDTO)
        {
            try
            {
                var existingReportType = await _reportTypeRepository.GetByIdAsync(id);
                if(existingReportType == null)
                {
                    return new OperationResult(true, "Report type not found", StatusCodes.Status404NotFound);
                }
                var reportType = _mapper.Map<ReportType>(reportTypeDTO);
                reportType.CreatedOn = existingReportType.CreatedOn;
                reportType.CreatedById = existingReportType.CreatedById;
                reportType.ModifiedById = existingReportType.ModifiedById;
                reportType.ModifiedOn = existingReportType.ModifiedOn;
                reportType.IsDeleted = existingReportType.IsDeleted;
                var isNameChange = reportType.Name != existingReportType.Name;
                EditHelper<ReportType>.SetModifiedIfNecessary(reportType, isNameChange, existingReportType, _userId);
                await _reportTypeRepository.UpdateAsync(reportType);
                return new OperationResult(true, "Report type update succesfully", StatusCodes.Status200OK);
            }
            catch (DbUpdateException dbEx)
            {
                var dbExMessage = dbEx.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, dbExMessage, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                var exMessage = ex.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }


        public async Task<OperationResult> DeletedByIdAsync(int id)
        {
            try
            {
                var reportType = await _reportTypeRepository.GetByIdAsync(id);
                if (reportType == null)
                {
                    return new OperationResult(false, "Report type not found", StatusCodes.Status404NotFound);
                }
                reportType.ModifiedById = _userId;
                reportType.ModifiedOn = DateTime.Now;
                reportType.IsDeleted = !reportType.IsDeleted;
                await _reportTypeRepository.UpdateAsync(reportType);
                return new OperationResult(true, "Report type deleted succesfully", StatusCodes.Status200OK);
            }
            catch (DbUpdateException dbEx)
            {
                var dbExMessage = dbEx.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, dbExMessage, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                var exMessage = ex.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }

        public async Task<OperationResult> GetAllAsync()
        {
            var reportTypes = await _reportTypeRepository.GetAllAsync();
            if(reportTypes.Count == 0)
            {
                return new OperationResult(false, message: "List empty", statusCode: StatusCodes.Status204NoContent);
            }
            var reportTypeVMs = _mapper.Map<List<ReportTypeVM>>(reportTypes);
            return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: reportTypeVMs);
        }

        public async Task<OperationResult> GetByIdAsync(int id)
        {
            var reportType = await _reportTypeRepository.GetByIdAsync(id);
            if(reportType == null)
            {
                return new OperationResult(false, "Report type not found", StatusCodes.Status404NotFound);
            }
            var reportTypeVM = _mapper.Map<ReportTypeVM>(reportType);
            return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: reportTypeVM);
        }

    }
}
