using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GoWheels_WebAPI.Models.Entities
{
    public class PostImage
    {
        public int Id { get; set; }
        public required string Url { get; set; }
        public int PostId { get; set; }
        [ValidateNever]
        public Post Post { get; set; } = null!;
    }
}
