using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Repositories.Interface
{
    public interface IInvoiceRepository : IGenericRepository<Invoice>
    {
        List<Invoice> GetAllByUserId(string userId);
        List<Invoice> GetAllByDriver(string userId);
        List<Invoice> GetAllByPostOwner(string userId);
        List<Invoice> GetAllRefundInvoices();
        Invoice GetByBookingId(int bookingId);

    }
}
