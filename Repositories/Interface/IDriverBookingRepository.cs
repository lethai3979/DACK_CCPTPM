using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Repositories.Interface
{
    public interface IDriverBookingRepository : IGenericRepository<DriverBooking>
    {
        List<DriverBooking> GetAllByUserId(string userId);
        DriverBooking GetByBookingId(int id);

    }
}
