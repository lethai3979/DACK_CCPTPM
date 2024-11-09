using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Repositories
{
    public class DriverRepository : IGenericRepository<Driver>
    {
        private readonly ApplicationDbContext _context;

        public DriverRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Driver>> GetAllAsync()
            => await _context.Drivers.Include(d => d.User).Where(d => !d.IsDeleted).ToListAsync();

        public async Task<Driver> GetByIdAsync(int id)
            => await _context.Drivers.FirstOrDefaultAsync(d => !d.IsDeleted && d.Id == id) 
                                                        ?? throw new NullReferenceException("Driver not found");
        public async Task<Driver> GetByUserIdAsync(string userId)
            => await _context.Drivers.FirstOrDefaultAsync(d => !d.IsDeleted && d.UserId == userId) 
                                                        ?? throw new NullReferenceException("Driver not found");

        public async Task AddAsync(Driver driver)
        {
            await _context.AddAsync(driver);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Driver driver)
        {
            var existingDriver = _context.ChangeTracker.Entries<Driver>()
                                                 .FirstOrDefault(e => e.Entity.Id == driver.Id);

            //detached same id obj
            if (existingDriver != null)
            {
                _context.Entry(existingDriver.Entity).State = EntityState.Detached;
            }

            _context.Entry(driver).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            //detached tracking obj after modified
            _context.Entry(driver).State = EntityState.Detached;
        }

        public async Task DeleteAsync(Driver driver)
        {
            _context.Entry(driver).State = EntityState.Modified;
            driver.IsDeleted = true;
            await _context.SaveChangesAsync();
        }





    }
}
