using GoWheels_WebAPI.Models.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GoWheels_WebAPI.Models.ViewModels
{
    public class SalePromotionVM : BaseModelVM
    {
        public string? Content { get; set; }
        public decimal DiscountValue { get; set; }
        public DateTime ExpiredDate { get; set; }
        public List<PostPromotionVM> PostPromotionVMs { get; set;} = new List<PostPromotionVM>();
    }
}
