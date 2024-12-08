using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface IInvoiceService
    {
        Task<List<Invoice>> GetAllAsync();
        Task<List<Invoice>> GetPersonalInvoicesAsync();
        Task<List<Invoice>> GetAllByDriverAsync();
        Task<Invoice> GetByIdAsync(int id);
        Task<Invoice> GetByBookingIdAsync(int bookingId);
        Task<Invoice> GetByDriverBookingIdAsync(int driverBookingId);
        Task<List<Invoice>> GetAllRefundInvoicesAsync();
        Task CreateInvoiceAsync(int bookingId);
        Task AddDriverToInvocieAsync(Booking booking, DriverBooking driverBooking);
        Task UpdateCancelDriverBookingAsync(Invoice invoice, decimal driverBookingTotal);
        Task UpdateAsync(Invoice invoice);
        Task RefundAsync(Booking booking, bool isAccept);
        Task RefundReportedBookingAsync(Booking booking);
        Task<string> ProcessMomoPaymentAsync(Invoice invoice);
        Task ProcessReturnUrlAsync(IQueryCollection queryParams);
    }
}
