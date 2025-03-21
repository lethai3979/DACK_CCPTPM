using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface IReportTypeService
    {
        List<ReportType> GetAll();
        ReportType GetById(int id);
        void Add(ReportType reportType);
        void Update(int id, ReportType reportType);
        void DeletedById(int id);
    }
}
