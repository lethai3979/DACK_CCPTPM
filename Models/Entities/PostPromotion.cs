using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GoWheels_WebAPI.Models.Entities
{
    public class PostPromotion
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        [ValidateNever]
        public Post Post { get; set; } = null!;
        public int PromotionId { get; set; }
        [ValidateNever]
        public Promotion Promotion { get; set; } = null!;
        public bool IsDeleted { get; set; }
    }
}
