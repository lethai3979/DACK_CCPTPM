using GoWheels_WebAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoWheels_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalePromotionTypeController : ControllerBase
    {
        private readonly ISalePromotionTypeRepository _salePromotionTypeRepository;
        public SalePromotionTypeController(ISalePromotionTypeRepository salePromotionTypeRepository)
        {
            _salePromotionTypeRepository = salePromotionTypeRepository;
        }
        [HttpPost("SeedingItem_SalePromotionTypes")]
        public async Task<IActionResult> SeedSalePromotionTypes()
        {
            await _salePromotionTypeRepository.SeedSalePromotionTypeAsync();
            return Ok("Sale Promotion Types Seeded Sucessfully!");
        }
    }
}
