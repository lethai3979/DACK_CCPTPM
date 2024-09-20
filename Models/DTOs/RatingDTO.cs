using GoWheels_WebAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace GoWheels_WebAPI.Models.DTOs
{
    public class RatingDTO
    {
        [Required(ErrorMessage = "Comment is required")]
        public string? Comment { get; set; }

        [Required(ErrorMessage = "Point is required")]
        [Range(1, 5, ErrorMessage = "Point must be between 1 and 5")]
        public float Point { get; set; }

        [Required(ErrorMessage = "PostId is required")]
        public int PostId { get; set; }
        public required string UserId { get; set; }
    }
}
