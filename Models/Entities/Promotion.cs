using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoWheels_WebAPI.Models.Entities
{
    public class Promotion : BaseModel
    {
        [Required]
        public required string Content { get; set; }
        [Required]
        public decimal DiscountValue { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = true)]
        [Required]
        public required DateTime ExpiredDate { get; set; }
        public ICollection<PostPromotion> PostPromotions { get; set; } = new List<PostPromotion>();
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public bool IsAdminPromotion { get; set; }
    }
}
