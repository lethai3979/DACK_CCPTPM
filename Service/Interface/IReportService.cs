using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface IReportService
    {
        Task<List<Report>> GetAllAsync();
        Task<Report> GetByIdAsync(int id);
        Task CreateReportAsync(Report report);
        Task ConfirmReportAsync(int id, bool isAccept);
    }
}
