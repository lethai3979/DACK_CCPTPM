using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Models.ViewModels
{
    public class NotifyVM
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public int BookingId { get; set; }
        public DateTime CreateOn { get; set; }
        public bool IsRead { get; set; }
        public UserVM User { get; set; } = null!;
    }
}
