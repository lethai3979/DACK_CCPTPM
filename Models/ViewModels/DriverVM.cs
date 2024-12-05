using System.Text.Json.Serialization;

namespace GoWheels_WebAPI.Models.ViewModels
{
    public class DriverVM : UserVM
    {
        public double RatingPoint { get; set; }
        public required decimal PricePerHour { get; set; }
    }
}
