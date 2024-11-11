namespace GoWheels_WebAPI.Models.Entities
{
    public class UserNotify
    {
        public int Id { get; set; }
        public required string UserId { get; set; }
        public ApplicationUser user { get; set; } = null!;
        public required int NotifyId { get; set; }
        public Notify Notify { get; set; } = null!;

    }
}
