using System.ComponentModel.DataAnnotations;

namespace GoWheels_WebAPI.Models.Entities
{
    public class PromotionType
    {
        public int Id { get; set; }
        [Required]
        public required string Name { get; set; }
        public ICollection<Promotion> Promotions { get; set; } = new List<Promotion>();
        public bool IsDeleted {  get; set; }
    }
}
