using GoWheels_WebAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GoWheels_WebAPI.Models.DTOs
{
    public class PostDTO : BaseModelDTO
    {
        public required string Name { get; set; }
        public required string Image { get; set; }
        public List<PostImageDTO> Images { get; set; } = new List<PostImageDTO>();
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
        public int CarTypeId { get; set; }
        public string? CarTypeName { get; set; }
        public int CompanyId { get; set; }

        public string? CompanyName { get; set; }
        public string? UserId { get; set; }

        public string? UserName { get; set; }
        public List<PostAmenityDTO> Amenities { get; set; } = new List<PostAmenityDTO>();
        public List<int> AmenitiesIdList { get; set; } = new List<int>();
    }
}
