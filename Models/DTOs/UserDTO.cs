namespace GoWheels_WebAPI.Models.DTOs
{
    public class UserDTO
    {
        public string? Name { get; set; }
        public IFormFile? License { get; set; }
        public IFormFile? CIC { get; set; }
        public IFormFile? Image { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
