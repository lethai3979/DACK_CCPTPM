using System.ComponentModel.DataAnnotations;

namespace GoWheels_WebAPI.Models.Entities
{
    public class Amenity : BaseModel
    {
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string IconImage { get; set; }
    }
}
