using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories.Interface;
using GoWheels_WebAPI.Service.Interface;
using GoWheels_WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GoWheels_WebAPI.Service
{
    public class ReportTypeService : IReportTypeService
    {
        private readonly IGenericRepository<ReportType> _reportTypeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _userId;
        public ReportTypeService(IGenericRepository<ReportType> reportTypeRepository, IHttpContextAccessor httpContextAccessor)
        {
            _reportTypeRepository = reportTypeRepository;
            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor.HttpContext?.User?
                     .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
        }

        public List<ReportType> GetAll()
            => _reportTypeRepository.GetAll();


        public ReportType GetById(int id)
            => _reportTypeRepository.GetById(id);

        public void Add(ReportType reportType)
        {
            try
            {
                reportType.CreatedById = _userId;
                reportType.CreatedOn = DateTime.Now;
                reportType.IsDeleted = false;
                _reportTypeRepository.Add(reportType);
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

        public void Update(int id, ReportType reportType)
        {
            try
            {
                var existingReportType = _reportTypeRepository.GetById(id);
                reportType.CreatedOn = existingReportType.CreatedOn;
                reportType.CreatedById = existingReportType.CreatedById;
                reportType.ModifiedById = existingReportType.ModifiedById;
                reportType.ModifiedOn = existingReportType.ModifiedOn;
                reportType.IsDeleted = existingReportType.IsDeleted;
                var isNameChange = reportType.Name != existingReportType.Name;
                EditHelper<ReportType>.SetModifiedIfNecessary(reportType, isNameChange, existingReportType, _userId);
                _reportTypeRepository.Update(reportType);
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


        public void DeletedById(int id)
        {
            try
            {
                var reportType = _reportTypeRepository.GetById(id);
                reportType.ModifiedById = _userId;
                reportType.ModifiedOn = DateTime.Now;
                reportType.IsDeleted = !reportType.IsDeleted;
                _reportTypeRepository.Update(reportType);
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
