using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Repositories
{
    public class CompanyRepository : IGenericRepository<Company>
    {
        private readonly ApplicationDbContext _context;

        public CompanyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddCompanyDetailAsync(int companyId, List<int> carTypeIds)
        {
            foreach (var carTypeId in carTypeIds)
            {
                var carTypeDetail = new CarTypeDetail();
                carTypeDetail.CarTypeId = carTypeId;
                carTypeDetail.CompanyId = companyId;
                await _context.CarTypeDetails.AddAsync(carTypeDetail);
            }
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(Company company)
        {
            await _context.Companies.AddAsync(company);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Company company)
        {
            _context.Entry(company).State = EntityState.Modified;
            company.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Company>> GetAllAsync()
            => await _context.Companies.AsNoTracking()
                                        .Include(c => c.CarTypeDetail.Where(ctd => !ctd.CarType.IsDeleted))
                                        .ThenInclude(c => c.CarType)
                                        .ToListAsync();

        public async Task<Company> GetByIdAsync(int id)
            => await _context.Companies.AsNoTracking()
                                        .Include(c => c.CarTypeDetail.Where(ctd => !ctd.CarType.IsDeleted))
                                        .ThenInclude(c => c.CarType)
                                        .FirstOrDefaultAsync(c => c.Id == id) 
                                        ?? throw new NullReferenceException("Company not found");

        public async Task UpdateAsync(Company company)
        {
            //Check if there's any obj with same id being tracked
            var existingCompany = _context.ChangeTracker.Entries<Company>()
                                         .FirstOrDefault(e => e.Entity.Id == company.Id);

            //detached same id obj
            if (existingCompany != null)
            {
                _context.Entry(existingCompany.Entity).State = EntityState.Detached;
            }

            _context.Companies.Attach(company);  // Attach target modified obj to context 
            _context.Entry(company).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            //detached tracking obj after modified
            _context.Entry(company).State = EntityState.Detached;
        }
    }
}
