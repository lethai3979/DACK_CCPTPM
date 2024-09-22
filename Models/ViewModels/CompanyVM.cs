using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GoWheels_WebAPI.Models.ViewModels
{
    public class CompanyVM : BaseModelVM
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? IconImage { get; set; }

        [JsonPropertyOrder(100)]
        public List<CarTypeDetailVM> carTypeDetail { get; set; } = new List<CarTypeDetailVM>();
        public ICollection<PostVM> Posts { get; set; } = new List<PostVM>();
        public List<int> CarTypeIds { get; set; } = new List<int>();

    }
}
