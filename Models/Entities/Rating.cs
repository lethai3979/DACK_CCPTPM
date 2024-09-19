using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GoWheels_WebAPI.Models.Entities
{
    public class Rating : BaseModel
    {
        public string? Comment { get; set; }
        public required float Point { get; set; }
        public required int PostId { get; set; }
        [ValidateNever]
        public Post Post { get; set; } = null!;

        public required string UserId { get; set; }
        [ValidateNever]
        public ApplicationUser User { get; set; } = null!;
    }
}
