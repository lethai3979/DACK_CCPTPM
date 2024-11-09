using GoWheels_WebAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GoWheels_WebAPI.Models.ViewModels
{
    public class InvoiceVM : BaseModel
    {
        public decimal Total { get; set; }
        public DateTime ReturnOn { get; set; }
        public BookingVM Booking { get; set; } = null!;
        public DriverBookingVM DriverBooking { get; set; } = null!;
    }
}
