using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Repositories.Interface
{
    public interface ICarTypeDetailRepository
    {
        Task<List<CarTypeDetail>> GetCarTypeDetails(int id);
        Task<List<CarTypeDetail>> GetCompanyDetails(int id);
        Task AddCompaniesListAsync(int carTypeId, List<int> companyIds);
        Task AddCarTypesListAsync(int companyId, List<int> carTypeIds);
        Task ClearCarTypeDetailsAsync(int carTypeId);
        Task ClearCompanyDetailsAsync(int companyId);
    }
}
