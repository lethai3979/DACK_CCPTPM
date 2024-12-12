using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface IInvoiceService
    {
        List<Invoice> GetAll();
        List<Invoice> GetPersonalInvoices();
        List<Invoice> GetAllByDriver();
        Invoice GetById(int id);
        Invoice GetByBookingId(int bookingId);
        Invoice GetByDriverBookingId(int driverBookingId);
        List<Invoice> GetAllRefundInvoices();
        void CreateInvoice(int bookingId);
        void AddDriverToInvocie(Booking booking, DriverBooking driverBooking);
        void UpdateCancelDriverBooking(Invoice invoice, decimal driverBookingTotal);
        void Update(Invoice invoice);
        void Refund(Booking booking, bool isAccept);
        void RefundReportedBooking(Booking booking);
        Task<string> ProcessMomoPayment(Invoice invoice);
        void ProcessReturnUrl(IQueryCollection queryParams);
        //Task CalculateRevenuesByYears
    }
}
