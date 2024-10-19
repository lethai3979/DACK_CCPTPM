using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Repositories
{
    public class ReportRepository : IGenericRepository<Report>
    {
        private readonly ApplicationDbContext _context;

        public ReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Report report)
        {
            await _context.Reports.AddAsync(report);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Report report)
        {
            var existingReport = await _context.ReportTypes.AsNoTracking()
                                     .FirstOrDefaultAsync(p => p.Id == report.Id);

            if (existingReport == null)
            {
                throw new KeyNotFoundException($"Report with ID {report.Id} not found.");
            }

            // Gán lại trạng thái cho đối tượng là modified và lưu các thay đổi
            _context.Entry(report).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Report report)
        {
            _context.Entry(report).State = EntityState.Modified;
            report.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Report>> GetAllAsync()
            => await _context.Reports.AsNoTracking()
                                        .Include(r => r.Post)
                                        .Include(r => r.ReportType)
                                        .Where(r => !r.IsDeleted)
                                        .ToListAsync();

        public async Task<Report> GetByIdAsync(int id)
            => await _context.Reports.AsNoTracking()
                                        .Include(r => r.Post)
                                        .Include(r => r.ReportType)
                                        .FirstOrDefaultAsync(r => !r.IsDeleted && r.Id == id)
                                        ?? throw new NullReferenceException("Report not found");


    }
}
