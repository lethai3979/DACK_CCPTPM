using System.ComponentModel.DataAnnotations;

namespace GoWheels_WebAPI.Models.DTOs
{
    public class FavoriteDTO
    {
        public int Id { get; set; }
        [Required]
        public int PostId { get; set; }
    }
}
