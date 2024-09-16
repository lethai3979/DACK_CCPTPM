using GoWheels_WebAPI.Interfaces;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.DTOs.SalePromotionDTOs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Service;
using GoWheels_WebAPI.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace GoWheels_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalePromotionController : ControllerBase
    {
        private readonly SalePromotionService _salePromotionService;
        public SalePromotionController(SalePromotionService salePromotionService)
        {
            _salePromotionService = salePromotionService;
        }
        [HttpGet("GetAllSalePromotion")]
        public async Task<ActionResult<OperationResult>> GetAllAsync()
        {
            var result = await _salePromotionService.GetAllAsync();
            return result;
        }
        [HttpGet("GetSalePromotion/{id}")]
        public async Task<ActionResult<OperationResult>> GetByIdAsync(int id)
        {
            var result = await _salePromotionService.GetByIdAsync(id);
            return result;
        }
        [HttpPost("AddSalePromotion")]
        public async Task<ActionResult<OperationResult>> AddAsync([FromBody] SalePromotionDto salePromotionDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _salePromotionService.AddAsync(salePromotionDto);
                return result;
            }
            return BadRequest("Sale Promotion data invalid");
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<OperationResult>> DeleteAsync(int id)
        {
            var result = await _salePromotionService.DeletedByIdAsync(id);
            return result;
        }
    }
}
