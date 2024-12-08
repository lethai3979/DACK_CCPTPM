using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface IDriverBookingService
    {
        Task<List<DriverBooking>> GetAllByUserIdAsync();
        Task<DriverBooking> GetByIdAsync(int id);
        Task<DriverBooking> GetByBookingIdAsync(int id);
        Task AddDriverBookingAsync(Booking booking);
        Task UpdateAsync(DriverBooking driverBooking);

    }
}
