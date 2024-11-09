using System.Text.Json.Serialization;

namespace GoWheels_WebAPI.Models.ViewModels
{
    public class DriverVM
    {
        [JsonPropertyOrder(1)]
        public UserVM User { get; set; } = null!;
        public double RatingPoint { get; set; }
        public required decimal PricePerHour { get; set; }
    }
}
