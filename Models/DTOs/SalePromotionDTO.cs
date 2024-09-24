using GoWheels_WebAPI.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoWheels_WebAPI.Models.DTOs
{
    public class SalePromotionDTO
    {
        public int Id { get; set; }

        [Required]
        public required string Content { get; set; }
        [Required]
        [Range(0.01, 1, ErrorMessage = "Discount value must be between 0.01 and 1.")]
        public decimal DiscountValue { get; set; }
        [Required]
        public required DateTime ExpiredDate { get; set; }
    }
}
