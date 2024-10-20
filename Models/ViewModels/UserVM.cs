namespace GoWheels_WebAPI.Models.ViewModels
{
    public class UserVM
    {
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? License { get; set; }
        public string? Image { get; set; }
        public DateTime? Birthday { get; set; }
        public string? Role { get; set; }
    }
}
