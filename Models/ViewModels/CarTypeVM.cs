using GoWheels_WebAPI.Models.Entities;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GoWheels_WebAPI.Models.ViewModels
{
    public class CarTypeVM : BaseModelVM
    {
        [Required]
        public required string Name { get; set; }

        [JsonPropertyOrder(100)]
        public List<CarTypeDetailVM> carTypeDetail { get; set; } = new List<CarTypeDetailVM>();

        public ICollection<PostVM> Posts { get; set; } = new List<PostVM>();
    }
}
