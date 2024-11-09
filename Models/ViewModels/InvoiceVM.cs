using GoWheels_WebAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace GoWheels_WebAPI.Models.ViewModels
{
    public class InvoiceVM : BaseModelVM
    {
        public decimal Total { get; set; }
        public DateTime ReturnOn { get; set; }
        [JsonPropertyOrder(-100)]
        public BookingVM Booking { get; set; } = null!;
        public DriverBookingVM DriverBooking { get; set; } = null!;
    }
}
