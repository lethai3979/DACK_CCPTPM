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
            => await _context.Companies.Where(c => !c.IsDeleted).Include(c => c.CarTypeDetail).ThenInclude(c => c.CarType).ToListAsync();

        public async Task<Company?> GetByIdAsync(int id)
            => await _context.Companies.Where(c => !c.IsDeleted).FirstOrDefaultAsync(c => c.Id == id);

        public Task UpdateAsync(Company entity, Company newEntity)
        {
            throw new NotImplementedException();
        }
    }
}
