using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface ICarTypeService
    {
        Task<List<CarType>> GetAllAsync();
        Task<CarType> GetByIdAsync(int id);
        Task AddAsync(CarType carType, List<int> companyList);
        Task DeleteByIdAsync(int id);
        Task UpdateAsync(int id, CarType carType, List<int> companyIds);

    }
}
