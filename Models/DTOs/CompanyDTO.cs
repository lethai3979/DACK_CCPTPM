using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GoWheels_WebAPI.Models.DTOs
{
    public class CompanyDTO : BaseModelDTO
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? IconImage { get; set; }

        [JsonPropertyOrder(100)]
        public List<CarTypeDetailDTO> carTypeDetail { get; set; } = new List<CarTypeDetailDTO>();
        public ICollection<PostDTO> Posts { get; set; } = new List<PostDTO>();
        public List<int> CarTypeIds { get; set; } = new List<int>();

    }
}
