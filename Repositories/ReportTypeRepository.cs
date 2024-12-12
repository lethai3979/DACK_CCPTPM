using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Repositories
{
    public class ReportTypeRepository : IGenericRepository<ReportType>
    {
        private readonly ApplicationDbContext _context;

        public ReportTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(ReportType reportType)
        {
            _context.ReportTypes.Add(reportType);
            _context.SaveChanges();
        }

        public void Update(ReportType reportType)
        {
            var existingReportType = _context.ReportTypes.AsNoTracking()
                                     .FirstOrDefault(p => p.Id == reportType.Id);

            if (existingReportType == null)
            {
                throw new KeyNotFoundException($"Report type with ID {reportType.Id} not found.");
            }

            // Gán lại trạng thái cho đối tượng là modified và lưu các thay đổi
            _context.Entry(reportType).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(ReportType reportType)
        {
            var existingReport = _context.Reports.Where(r => r.ReportTypeId == reportType.Id).ToList();
            _context.Reports.RemoveRange(existingReport);
            _context.ReportTypes.Remove(reportType);
            _context.SaveChanges();
        }

        public List<ReportType> GetAll()
            => _context.ReportTypes.AsNoTracking().ToList();

        public ReportType GetById(int id)
            => _context.ReportTypes.AsNoTracking()
                                            .Include(r => r.Reports)
                                            .FirstOrDefault(r => r.Id == id)
                                            ?? throw new NullReferenceException("Report type not found");
    }
}
