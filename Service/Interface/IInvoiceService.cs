using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface IInvoiceService
    {
        List<Invoice> GetAll();
        List<Invoice> GetPersonalInvoices();
        //List<Invoice> GetAllByDriver();
        Invoice GetById(int id);
        Invoice GetByBookingId(int bookingId);
        List<Invoice> GetAllRefundInvoices();
        void CreateInvoice(int bookingId);
        void Update(Invoice invoice);
        void Refund(Booking booking, bool isAccept);
        void RefundReportedBooking(Booking booking);
        Task<string> ProcessMomoPayment(Booking booking);
        void ProcessReturnUrl(IQueryCollection queryParams);
        //Task CalculateRevenuesByYears
        List<(int month, decimal revenue)> CalculateRevenuesByMonth(int year);
    }
}
