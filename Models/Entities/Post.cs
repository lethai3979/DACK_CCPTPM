using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GoWheels_WebAPI.Models.Entities
{
    public class Post : BaseModel
    {
        public required string Name { get; set; }
        public required string Image { get; set; }
        public List<PostImage> Images { get; set; } = new List<PostImage>();
        public string? Description { get; set; }
        public required int Seat { get; set; }
        public string? RentLocation { get; set; }
        public bool HasDriver { get; set; }
        public required decimal PricePerHour { get; set; }
        public required decimal PricePerDay { get; set; }
        public bool Gear { get; set; }
        public string? Fuel { get; set; }
        public decimal FuelConsumed { get; set; }
        public int RideNumber { get; set; }
        public float AvgRating { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsHidden { get; set; }
        public int CarTypeId { get; set; }
        [ValidateNever]
        public CarType CarType { get; set; } = null!;
        public int CompanyId { get; set; }
        [ValidateNever]
        public Company Company { get; set; } = null!;
        public string? UserId { get; set; }
        [ValidateNever]
        public ApplicationUser User { get; set; } = null!;
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
        public ICollection<Booking> Booking { get; set; } = new List<Booking>();
        public ICollection<PostAmenity> PostAmenities { get; set; } = new List<PostAmenity>();
        public ICollection<PostPromotion> PostPromotions { get; set; } = new List<PostPromotion>();
        public ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();
        public ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
