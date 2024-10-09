using AutoMapper;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Repositories;
using GoWheels_WebAPI.Repositories.Interface;
using GoWheels_WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GoWheels_WebAPI.Service
{
    public class ReportService
    {
        private readonly ReportRepository _reportRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly string _userId;

        public ReportService(ReportRepository reportRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _reportRepository = reportRepository;
            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor.HttpContext?.User?
                   .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
            _mapper = mapper;
        }

        public async Task<OperationResult> AddAsync(ReportDTO reportDTO)
        {
            try
            {
                var report = _mapper.Map<Report>(reportDTO);
                report.IsDeleted = false;
                report.CreatedById = _userId;
                report.CreatedOn = DateTime.Now;
                await _reportRepository.AddAsync(report);
                return new OperationResult(true, "Report succesfully", StatusCodes.Status200OK);
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

        public async Task<OperationResult> DeletedAsync(int id)
        {
            try
            {
                var report = await _reportRepository.GetByIdAsync(id);
                if (report == null)
                {
                    return new OperationResult(false, "Report not found", StatusCodes.Status404NotFound);
                }
                report.IsDeleted = true;
                report.ModifiedById = _httpContextAccessor.HttpContext?.User?
                    .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
                report.ModifiedOn = DateTime.Now;
                await _reportRepository.UpdateAsync(report);
                return new OperationResult(true, "Report delete succesfully", StatusCodes.Status200OK);
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
            var reports = await _reportRepository.GetAllAsync();
            if(reports.Count == 0)
            {
                return new OperationResult(false, "List empty", StatusCodes.Status404NotFound);
            } 
            var reportVMs = _mapper.Map<List<ReportVM>>(reports);
            return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: reportVMs);
        }

        public async Task<OperationResult> GetByIdAsync(int id)
        {
            var report = await _reportRepository.GetByIdAsync(id);
            if (report == null)
            {
                return new OperationResult(false, "Report not found", StatusCodes.Status404NotFound);
            }
            var reportVM = _mapper.Map<ReportVM>(report);
            return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: reportVM);
        }

    }
}
