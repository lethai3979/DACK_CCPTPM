using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GoWheels_WebAPI.Models.Entities
{
    public class PostAmenity
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        [ValidateNever]
        public Post Post { get; set; } = null!;
        public int AmenityId { get; set; }
        [ValidateNever]
        public Amenity Amenity { get; set; } = null!;
        public bool IsDeleted { get; set; }
    }
}
