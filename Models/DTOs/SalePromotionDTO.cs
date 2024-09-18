using GoWheels_WebAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GoWheels_WebAPI.Models.DTOs
{
    public class SalePromotionDTO : BaseModelDTO
    {
        public required string Content { get; set; }
        public decimal DiscountValue { get; set; }
        public required DateTime ExpiredDate { get; set; }
        public int PromotionId { get; set; }
        public string? PromotionName { get; set; }
    }
}
