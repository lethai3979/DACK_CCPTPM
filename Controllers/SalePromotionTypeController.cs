using GoWheels_WebAPI.Interfaces;
using GoWheels_WebAPI.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoWheels_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalePromotionTypeController : ControllerBase
    {
        private readonly SalePromotionTypeRepository _salePromotionTypeRepository;
        public SalePromotionTypeController(SalePromotionTypeRepository salePromotionTypeRepository)
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
