namespace GoWheels_WebAPI.Models.ViewModels
{
    public class DriverVM
    {
        public double RatingPoint { get; set; }
        public required decimal PricePerHour { get; set; }
        public UserVM User { get; set; } = null!;
    }
}
