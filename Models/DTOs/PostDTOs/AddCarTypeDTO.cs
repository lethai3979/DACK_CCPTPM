using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GoWheels_WebAPI.Models.DTOs.PostDTOs
{
    public class AddCarTypeDTO
    {
        [Required]
        public required string Name { get; set; }
        public List<int> CompanyIds { get; set; } = new List<int>();
    }
}
