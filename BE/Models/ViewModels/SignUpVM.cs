using System.ComponentModel.DataAnnotations;

namespace GoWheels_WebAPI.Models.ViewModels
{
    public class SignUpVM
    {
        [Required]
        public required string UserName { get; set; }
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
        [Required]
        public required string PhoneNumber { get; set; }
    }
}
