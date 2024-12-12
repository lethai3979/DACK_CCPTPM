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

        public void Add(Company company)
        {
            _context.Companies.Add(company);
            _context.SaveChanges();
        }

        public void Delete(Company company)
        {
            _context.Entry(company).State = EntityState.Modified;
            company.IsDeleted = true;
            _context.SaveChanges();
        }

        public List<Company> GetAll()
            => _context.Companies.AsNoTracking()
                                        .Include(c => c.CarTypeDetail.Where(ctd => !ctd.CarType.IsDeleted))
                                        .ThenInclude(c => c.CarType)
                                        .ToList();

        public Company GetById(int id)
            => _context.Companies.AsNoTracking()
                                        .Include(c => c.CarTypeDetail.Where(ctd => !ctd.CarType.IsDeleted))
                                        .ThenInclude(c => c.CarType)
                                        .FirstOrDefault(c => c.Id == id) 
                                        ?? throw new NullReferenceException("Company not found");

        public void Update(Company company)
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

            _context.SaveChanges();

            //detached tracking obj after modified
            _context.Entry(company).State = EntityState.Detached;
        }
    }
}
