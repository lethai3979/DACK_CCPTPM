using Microsoft.AspNetCore.Identity;

namespace GoWheels_WebAPI.Models.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? License { get; set; }
        public string? Image { get; set; }
        public DateTime? Birthday { get; set; }
        public int? ReportPoint { get; set; } = 0;
    }
}
