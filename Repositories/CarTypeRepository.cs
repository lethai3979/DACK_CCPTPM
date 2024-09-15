using AutoMapper;
using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace GoWheels_WebAPI.Repositories
{
    public class CarTypeRepository : IGenericRepository<CarType>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CarTypeRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddCarTypeDetail(int carTypeId, List<int> CompanyIds)
        {
            foreach (var companyId in CompanyIds)
            {
                var carTypeDetail = new CarTypeDetail();
                carTypeDetail.CarTypeId = carTypeId;
                carTypeDetail.CompanyId = companyId;
                await _context.CarTypeDetails.AddAsync(carTypeDetail);
            }
            await _context.SaveChangesAsync();             
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

        public async Task UpdateAsync(CarType carType, CarType newCarType)
        {
            _context.Entry(carType).State = EntityState.Modified;
            _mapper.Map(newCarType, carType);
            await _context.SaveChangesAsync();

        }

        public async Task<List<CarType>> GetAllAsync()
            => await _context.CarTypes.AsNoTracking().Where(c => !c.IsDeleted).ToListAsync();
        

        public async Task<CarType?> GetByIdAsync(int id)
            => await _context.CarTypes.Where (c => !c.IsDeleted).FirstOrDefaultAsync(c => c.Id == id);

    }
}
