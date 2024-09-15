using System.ComponentModel.DataAnnotations;

namespace GoWheels_WebAPI.Models.ViewModels
{
    public class LoginVM
    {
        [Required]
        public required string UserName { get; set; }
        [Required]
        public required string Password { get; set; }
    }
}
