using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface ICompanyService
    {
        Task<List<Company>> GetAllAsync();
        Task<Company> GetByIdAsync(int id);
        Task AddAsync(Company company, List<int> carTypeIds, IFormFile formFile);
        Task DeleteByIdAsync(int id);
        Task UpdateAsync(int id, Company company, List<int> carTypeIds, IFormFile formFile);
    }
}
