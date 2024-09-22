using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GoWheels_WebAPI.Models.DTOs
{
    public class CarTypeDTO
    {
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        public List<int> CompanyIds { get; set; } = new List<int>();
        public bool IsDeleted { get; set; } = false!;

    }
}
