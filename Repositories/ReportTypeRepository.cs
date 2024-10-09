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

        public async Task AddAsync(ReportType reportType)
        {
            await _context.ReportTypes.AddAsync(reportType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ReportType reportType)
        {
            var existingReportType = await _context.ReportTypes.AsNoTracking()
                                     .FirstOrDefaultAsync(p => p.Id == reportType.Id);

            if (existingReportType == null)
            {
                throw new KeyNotFoundException($"Report type with ID {reportType.Id} not found.");
            }

            // Gán lại trạng thái cho đối tượng là modified và lưu các thay đổi
            _context.Entry(reportType).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(ReportType reportType)
        {
            var existingReport = await _context.Reports.Where(r => r.ReportTypeId == reportType.Id).ToListAsync();
            _context.Reports.RemoveRange(existingReport);
            _context.ReportTypes.Remove(reportType);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ReportType>> GetAllAsync()
            => await _context.ReportTypes.AsNoTracking().ToListAsync();

        public async Task<ReportType?> GetByIdAsync(int id)
            => await _context.ReportTypes.AsNoTracking().Include(r => r.Reports).FirstOrDefaultAsync(r => r.Id == id);


    }
}
