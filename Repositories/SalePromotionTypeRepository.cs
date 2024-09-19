using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Repositories
{
    public class SalePromotionTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public SalePromotionTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task SeedSalePromotionTypeAsync()
        {
            if (!await _context.PromotionTypes.AnyAsync())
            {
                var seedingitem = new List<PromotionType>
                {
                    new PromotionType { Name = "Web-SalePromotion", CreatedById = "System", CreatedOn = DateTime.Now },
                    new PromotionType { Name = "User-SalePromotion", CreatedById = "System", CreatedOn = DateTime.Now }
                };
                await _context.PromotionTypes.AddRangeAsync(seedingitem);
                await _context.SaveChangesAsync();
            }
        }
    }
}