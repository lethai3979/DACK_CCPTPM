using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Repositories.Interface
{
    public interface ICarTypeDetailRepository
    {
        List<CarTypeDetail> GetCarTypeDetails(int id);
        List<CarTypeDetail> GetCompanyDetails(int id);
        void AddCompaniesList(int carTypeId, List<int> companyIds);
        void AddCarTypesList(int companyId, List<int> carTypeIds);
        void ClearCarTypeDetails(int carTypeId);
        void ClearCompanyDetails(int companyId);
    }
}
