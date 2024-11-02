namespace GoWheels_WebAPI.Models.DTOs
{
    public class UserDTO
    {
        public string? Name { get; set; }
        public string? License { get; set; }
        public string? Image { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
