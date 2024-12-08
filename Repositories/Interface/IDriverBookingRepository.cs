using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Repositories.Interface
{
    public interface IDriverBookingRepository : IGenericRepository<DriverBooking>
    {
        Task<List<DriverBooking>> GetAllByUserIdAsync(string userId);
        Task<DriverBooking> GetByBookingIdAsync(int id);

    }
}
