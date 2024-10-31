
namespace GoWheels_WebAPI.Models.ViewModels
{
    public class PostPromotionVM
    {
        public int Id { get; set; }
        public int PromotionId { get; set; }
        public PromotionVM Promotion { get; set; } = null!;
        public int PostId { get; set; }
        public string? PostName { get; set; }
    }
}
