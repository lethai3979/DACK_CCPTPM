using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GoWheels_WebAPI.Models.DTOs
{
    public class CarTypeDTO
    {
        public int Id { get; set; }
        //[Required]
        public string? Name { get; set; }
        public List<int> CompanyIds { get; set; } = new List<int>();

    }
}
