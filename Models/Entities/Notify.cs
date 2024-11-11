namespace GoWheels_WebAPI.Models.Entities
{
    public class Notify 
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public required string CreatedById { get; set; }
        public required DateTime CreateOn { get; set; }
        public bool IsDeleted { get; set; }
        public ICollection<UserNotify> UserNotifications { get; set; } = new List<UserNotify>();
    }
}
