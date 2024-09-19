using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GoWheels_WebAPI.Models.Entities
{
    public class Favorite 
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        [ValidateNever]
        public Post Post { get; set; } = null!;
        public string? UserId { get; set; }
        [ValidateNever]
        public ApplicationUser User { get; set; } = null!;
        public bool IsDeleted { get; set; }
    }
}
