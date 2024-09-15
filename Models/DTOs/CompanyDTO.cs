using System.ComponentModel.DataAnnotations;

namespace GoWheels_WebAPI.Models.DTOs
{
    public class CompanyDTO : BaseModelDTO
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? IconImage { get; set; }
        public ICollection<int> CarTypeIds { get; set; } = new List<int>();

    }
}
