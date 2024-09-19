using System.ComponentModel.DataAnnotations;

namespace GoWheels_WebAPI.Models.Entities
{
    public class PromotionType : BaseModel
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        public ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
    }
}
