using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Models.DTOs.SalePromotionTypeDtos
{
    public class SalePromotionTypeDto
    {
        public required string Name { get; set; }
        public List<Promotion> promotions { get; set; }
    }
}
