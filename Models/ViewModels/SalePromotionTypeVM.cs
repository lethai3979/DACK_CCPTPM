using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Models.ViewModels
{
    public class SalePromotionTypeVM
    {
        public required string Name { get; set; }
        public List<Promotion> Promotions { get; set; } = new List<Promotion>();
    }
}
