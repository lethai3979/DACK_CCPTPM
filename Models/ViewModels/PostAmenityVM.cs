using GoWheels_WebAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GoWheels_WebAPI.Models.ViewModels
{
    public class PostAmenityVM
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string? PostName { get; set; }
        public int AmenityId { get; set; }
        public string? AmenityName { get; set; }
        public bool IsDeleted { get; set; }
    }
}
