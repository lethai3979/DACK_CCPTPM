namespace GoWheels_WebAPI.Models.Entities
{
    public class Driver : BaseModel
    {
        public double RatingPoint { get; set; }
        public required decimal PricePerHour { get; set; }
        public required string UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public ICollection<DriverBooking> DriverBookings { get; set; } = new List<DriverBooking>();
    }
}
