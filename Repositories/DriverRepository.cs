using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Repositories
{
    public class DriverRepository : IDriverRepository
    {
        private readonly ApplicationDbContext _context;

        public DriverRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<Driver> GetAll()
            => _context.Drivers.Include(d => d.User).Where(d => !d.IsDeleted).ToList();

        public Driver GetById(int id)
            => _context.Drivers.FirstOrDefault(d => !d.IsDeleted && d.Id == id)
                                                        ?? throw new NullReferenceException("Driver not found");
        public Driver GetByUserId(string userId)
            => _context.Drivers.FirstOrDefault(d => !d.IsDeleted && d.UserId == userId)
                                                        ?? throw new NullReferenceException("Driver not found");

        public void Add(Driver driver)
        {
            _context.Add(driver);
            _context.SaveChanges();
        }

        public void Update(Driver driver)
        {
            var existingDriver = _context.ChangeTracker.Entries<Driver>()
                                                 .FirstOrDefault(e => e.Entity.Id == driver.Id);

            //detached same id obj
            if (existingDriver != null)
            {
                _context.Entry(existingDriver.Entity).State = EntityState.Detached;
            }

            _context.Entry(driver).State = EntityState.Modified;
            _context.SaveChanges();

            //detached tracking obj after modified
            _context.Entry(driver).State = EntityState.Detached;
        }

        public void Delete(Driver driver)
        {
            _context.Entry(driver).State = EntityState.Modified;
            driver.IsDeleted = true;
            _context.SaveChanges();
        }
    }
}
