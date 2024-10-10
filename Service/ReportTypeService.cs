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

        public async Task<List<ReportType>> GetAllAsync()
        {
            var reportTypes = await _reportTypeRepository.GetAllAsync();
            if (reportTypes.Count == 0)
            {
                throw new NullReferenceException("List is empty");
            }
            return reportTypes;
        }

        public async Task<ReportType> GetByIdAsync(int id)
            => await _reportTypeRepository.GetByIdAsync(id);

        public async Task AddAsync(ReportType reportType)
        {
            try
            {
                reportType.CreatedById = _userId;
                reportType.CreatedOn = DateTime.Now;
                reportType.IsDeleted = false;
                await _reportTypeRepository.AddAsync(reportType);
            }
            catch (DbUpdateException dbEx)
            {
                throw new DbUpdateException(dbEx.InnerException!.Message);
            }
            catch (InvalidOperationException operationEx)
            {
                throw new InvalidOperationException(operationEx.InnerException!.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateAsync(int id, ReportType reportType)
        {
            try
            {
                var existingReportType = await _reportTypeRepository.GetByIdAsync(id);
                reportType.CreatedOn = existingReportType.CreatedOn;
                reportType.CreatedById = existingReportType.CreatedById;
                reportType.ModifiedById = existingReportType.ModifiedById;
                reportType.ModifiedOn = existingReportType.ModifiedOn;
                reportType.IsDeleted = existingReportType.IsDeleted;
                var isNameChange = reportType.Name != existingReportType.Name;
                EditHelper<ReportType>.SetModifiedIfNecessary(reportType, isNameChange, existingReportType, _userId);
                await _reportTypeRepository.UpdateAsync(reportType);
            }
            catch (DbUpdateException dbEx)
            {
                throw new DbUpdateException(dbEx.InnerException!.Message);
            }
            catch (InvalidOperationException operationEx)
            {
                throw new InvalidOperationException(operationEx.InnerException!.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task DeletedByIdAsync(int id)
        {
            try
            {
                var reportType = await _reportTypeRepository.GetByIdAsync(id);
                reportType.ModifiedById = _userId;
                reportType.ModifiedOn = DateTime.Now;
                reportType.IsDeleted = !reportType.IsDeleted;
                await _reportTypeRepository.UpdateAsync(reportType);
            }
            catch (DbUpdateException dbEx)
            {
                throw new DbUpdateException(dbEx.InnerException!.Message);
            }
            catch (InvalidOperationException operationEx)
            {
                throw new InvalidOperationException(operationEx.InnerException!.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
