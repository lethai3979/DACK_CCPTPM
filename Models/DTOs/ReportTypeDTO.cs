using System.ComponentModel.DataAnnotations;

namespace GoWheels_WebAPI.Models.DTOs
{
    public class ReportTypeDTO
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        public int ReportPoint { get; set; }
    }
}
