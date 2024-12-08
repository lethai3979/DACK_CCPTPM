using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface IDriverService
    {
        Task<List<Driver>> GetAllAsync();
        Task<Driver> GetByIdAsync(int id);
        Task<Driver> GetByUserIdAsync(string userId);
        Task AddAsync(ApplicationUser user);
        Task UpdateAsync(int id, Driver driver);
        Task DeleteByIdAsync(int id);
        Task SendNotifyToDrivers(Booking booking);
    }
}
