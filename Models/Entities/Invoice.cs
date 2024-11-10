using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Diagnostics.CodeAnalysis;

namespace GoWheels_WebAPI.Models.Entities
{
    public class Invoice: BaseModel
    {
        public required decimal PrePayment { get; set; }
        public decimal Total { get; set; }
        public required DateTime ReturnOn { get; set; }
        public bool RefundInvoice { get; set; }
        public bool IsPay { get; set; }
        public int BookingId { get; set; }
        [ValidateNever]
        public Booking Booking { get; set; } = null!;
        [AllowNull]
        public int? DriverBookingId { get; set; }
        public DriverBooking DriverBooking { get; set; } = null!;
    }
}
