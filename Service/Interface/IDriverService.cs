using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface IDriverService
    {
        List<Driver> GetAll();
        Driver GetById(int id);
        Driver GetByUserId(string userId);
        void Add(ApplicationUser user);
        void Update(int id, Driver driver);
        void DeleteById(int id);
        Task SendNotifyToDrivers(Booking booking);
    }
}
