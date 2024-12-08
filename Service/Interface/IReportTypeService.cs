using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface IReportTypeService
    {
        Task<List<ReportType>> GetAllAsync();
        Task<ReportType> GetByIdAsync(int id);
        Task AddAsync(ReportType reportType);
        Task UpdateAsync(int id, ReportType reportType);
        Task DeletedByIdAsync(int id);
    }
}
