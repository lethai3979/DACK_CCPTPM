using System.ComponentModel.DataAnnotations;

namespace GoWheels_WebAPI.Models.DTOs
{
    public class PostDTO
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Image { get; set; }
        public List<string> ImageUrls { get; set; } = new List<string>();
        public string? Description { get; set; }
        [Required]
        public int Seat { get; set; }
        public string? RentLocation { get; set; }
        [Required]
        public bool HasDriver { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public bool Gear { get; set; }
        [Required]
        public string? Fuel { get; set; }
        [Required]
        public decimal FuelConsumed { get; set; }
        [Required]
        public int CarTypeId { get; set; }

        [Required]
        public int CompanyId { get; set; }

        public List<int> PostAmenitiesIds { get; set; } = new List<int>();
    }
}
