using GoWheels_WebAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace GoWheels_WebAPI.Models.ViewModels
{
    public class RatingVM : BaseModelVM
    {
        public string? Comment { get; set; }

        public float Point { get; set; }
        public UserVM User { get; set; } = null!;
    }
}
