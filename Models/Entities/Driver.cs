using System.ComponentModel.DataAnnotations;

namespace GoWheels_WebAPI.Models.Entities
{
    public class Driver
    {
        [Key]
        public required string UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        [Required]
        public string? CreatedById { get; set; }
        [Required]
        public required DateTime CreatedOn { get; set; } = DateTime.Now;

        public string? ModifiedById { get; set; }

        public DateTime? ModifiedOn { get; set; }
        public int TrustLevel { get; set; }
        public required decimal PricePerHour { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public bool IsDeleted { get; set; }
    }
}
