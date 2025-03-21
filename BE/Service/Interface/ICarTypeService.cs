using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface ICarTypeService
    {
        List<CarType> GetAll();
        CarType GetById(int id);
        void Add(CarType carType, List<int> companyList);
        void DeleteById(int id);
        void Update(int id, CarType carType, List<int> companyIds);

    }
}
