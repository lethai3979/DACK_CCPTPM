using AutoMapper;
using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Repositories
{
    public class CarTypeDetailRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CarTypeDetailRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CarTypeDetail>> GetCarTypeDetails(int id)
            => await _context.CarTypeDetails.Include(c => c.Company).Where(c => c.CarTypeId == id).ToListAsync();


        public async Task AddCompaniesListAsync(int carTypeId, List<int> companyIds)
        {
            foreach (var companyId in companyIds)
            {
                var carTypeDetail = new CarTypeDetail();
                carTypeDetail.CarTypeId = carTypeId;
                carTypeDetail.CompanyId = companyId;
                await _context.CarTypeDetails.AddAsync(carTypeDetail);
            }
            await _context.SaveChangesAsync();
        }

        public async Task ClearCarTypeDetailsAsync(int carTypeId)
        {
            var carTypeDetailsToRemove = await _context.CarTypeDetails
                                                .Where(p => p.CarTypeId == carTypeId)
                                                .ToListAsync();
            foreach(var carTypeDetail in carTypeDetailsToRemove)
            {
                _context.CarTypeDetails.Remove(carTypeDetail);
            }    
            await _context.SaveChangesAsync();
        }
    }
}
