using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Repositories.Interface
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        Task<List<Invoice>> GetAllByUserIdAsync(string userId);
        Task<List<Invoice>> GetAllByDriverAsync(string userId);
        Task<List<Invoice>> GetAllRefundInvoicesAsync();
        Task<Invoice> GetByBookingIdAsync(int bookingId);
        Task<Invoice> GetByDriverBookingIdAsync(int driverBooking);
    }
}
