using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Repositories
{
    public class CarTypeDetailRepository : ICarTypeDetailRepository
    {
        private readonly ApplicationDbContext _context;

        public CarTypeDetailRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<CarTypeDetail> GetCarTypeDetails(int id)
            => _context.CarTypeDetails.Include(c => c.Company).Where(c => c.CarTypeId == id).ToList();

        public List<CarTypeDetail> GetCompanyDetails(int id)
            => _context.CarTypeDetails.Include(c => c.CarType).Where(c => c.CompanyId == id).ToList();

        public void AddCompaniesList(int carTypeId, List<int> companyIds)
        {
            foreach (var companyId in companyIds)
            {
                var carTypeDetail = new CarTypeDetail();
                carTypeDetail.CarTypeId = carTypeId;
                carTypeDetail.CompanyId = companyId;
                _context.CarTypeDetails.Add(carTypeDetail);
            }
            _context.SaveChanges();
        }

        public void AddCarTypesList(int companyId, List<int> carTypeIds)
        {
            foreach (var carTypeId in carTypeIds)
            {
                var carTypeDetail = new CarTypeDetail();
                carTypeDetail.CarTypeId = carTypeId;
                carTypeDetail.CompanyId = companyId;
                _context.CarTypeDetails.Add(carTypeDetail);
            }
            _context.SaveChanges();
        }
        public void ClearCarTypeDetails(int carTypeId)
        {
            var carTypeDetailsToRemove = _context.CarTypeDetails
                                                .Where(p => p.CarTypeId == carTypeId)
                                                .ToList();
            foreach (var carTypeDetail in carTypeDetailsToRemove)
            {
                _context.CarTypeDetails.Remove(carTypeDetail);
            }
            _context.SaveChanges();
        }

        public void ClearCompanyDetails(int companyId)
        {
            var carTypeDetailsToRemove = _context.CarTypeDetails
                                                .Where(p => p.CompanyId == companyId)
                                                .ToList();
            foreach (var carTypeDetail in carTypeDetailsToRemove)
            {
                _context.CarTypeDetails.Remove(carTypeDetail);
            }
            _context.SaveChanges();
        }
    }
}
