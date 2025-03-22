using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface ICompanyService
    {
        List<Company> GetAll();
        Company GetById(int id);
        void Add(Company company, List<int> carTypeIds, IFormFile formFile);
        void DeleteById(int id);
        void Update(int id, Company company, List<int> carTypeIds, IFormFile formFile);
    }
}
