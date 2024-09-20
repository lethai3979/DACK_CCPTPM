using GoWheels_WebAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GoWheels_WebAPI.Models.DTOs
{
    public class PostImageDTO
    {
        public int Id { get; set; }
        public required string Url { get; set; }
        public int PostId { get; set; }
        public string? PostName { get; set; }
    }
}
