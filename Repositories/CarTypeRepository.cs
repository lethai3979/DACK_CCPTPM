using AutoMapper;
using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories.Interface;
using GoWheels_WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace GoWheels_WebAPI.Repositories
{
    public class CarTypeRepository : IGenericRepository<CarType>
    {
        private readonly ApplicationDbContext _context;

        public CarTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task AddAsync(CarType carType)
        {
            await _context.CarTypes.AddAsync(carType);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(CarType carType)
        {
            _context.Entry(carType).State = EntityState.Modified;
            carType.IsDeleted = true;
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(CarType carType)
    {
        //Check if there's any obj with same id being tracked
        var existingCarType = _context.ChangeTracker.Entries<CarType>()
                                     .FirstOrDefault(e => e.Entity.Id == carType.Id);

        //detached same id obj
        if (existingCarType != null)
        {
            _context.Entry(existingCarType.Entity).State = EntityState.Detached;
        }

        _context.CarTypes.Attach(carType);  // Attach target modified obj to context 
        _context.Entry(carType).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        //detached tracking obj after modified
        _context.Entry(carType).State = EntityState.Detached;
    }

        public async Task<List<CarType>> GetAllAsync()
            => await _context.CarTypes.AsNoTracking()
                                        .Include(c => c.CarTypeDetail.Where(ctd => !ctd.CarType.IsDeleted))
                                        .ThenInclude(c => c.Company)
                                        .ToListAsync();
        

        public async Task<CarType?> GetByIdAsync(int id)
            => await _context.CarTypes.AsNoTracking()
                                        .Include(c => c.CarTypeDetail.Where(ctd => !ctd.CarType.IsDeleted))
                                        .ThenInclude(c => c.Company)
                                        .FirstOrDefaultAsync(c => c.Id == id);

    }
}
