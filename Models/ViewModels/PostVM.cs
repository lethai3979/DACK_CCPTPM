using GoWheels_WebAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GoWheels_WebAPI.Models.ViewModels
{
    public class PostVM : BaseModelVM
    {
        public required string Name { get; set; }
        public required string Image { get; set; }
        public List<PostImageVM> Images { get; set; } = new List<PostImageVM>();
        public string? Description { get; set; }
        public required int Seat { get; set; }
        public string? RentLocation { get; set; }
        public bool HasDriver { get; set; }
        public required decimal Price { get; set; }
        public bool Gear { get; set; }
        public string? Fuel { get; set; }
        public decimal FuelConsumed { get; set; }
        public int RideNumber { get; set; }
        public float AvgRating { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDisabled { get; set; }
        public string? CarTypeName { get; set; }
        public string? CompanyName { get; set; }
        public UserVM User { get; set; } = null!;
        public List<PostAmenityVM> PostAmenities { get; set; } = new List<PostAmenityVM>();
        public List<PostPromotionVM> PostPromotions { get; set; } = new List<PostPromotionVM>();
        public List<RatingVM> Ratings { get; set; } = new List<RatingVM>();
    }
}
