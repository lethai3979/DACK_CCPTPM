using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface IDriverBookingService
    {
        List<DriverBooking> GetAllByUserId();
        DriverBooking GetById(int id);
        DriverBooking GetByBookingId(int id);
        void AddDriverBooking(Booking booking);
        void Update(DriverBooking driverBooking);

    }
}
