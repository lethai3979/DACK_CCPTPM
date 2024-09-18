using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Models.DTOs
{
    public class SalePromotionTypeDTO
    {
        public required string Name { get; set; }
        public List<Promotion> Promotions { get; set; } = new List<Promotion>();
    }
}
