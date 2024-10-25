using AutoMapper;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Service;
using GoWheels_WebAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace GoWheels_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = ("Admin, User"))]
    public class SalePromotionController : ControllerBase
    {
        private readonly SalePromotionService _salePromotionService;
        private readonly IMapper _mapper;
        public SalePromotionController(SalePromotionService salePromotionService, IMapper mapper)
        {
            _salePromotionService = salePromotionService;
            _mapper = mapper;
        }
        [HttpGet("GetAll")]
        public async Task<ActionResult<OperationResult>> GetAllAsync()
        {
            try
            {
                var promotions = await _salePromotionService.GetAllAsync();
                var promotionVMs = _mapper.Map<List<SalePromotionVM>>(promotions);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: promotionVMs);
            }
            catch (NullReferenceException nullEx)
            {
                return new OperationResult(false, nullEx.Message, StatusCodes.Status204NoContent);
            }
            catch (AutoMapperMappingException mapperEx)
            {
                return new OperationResult(false, mapperEx.Message, StatusCodes.Status422UnprocessableEntity);
            }
            catch (Exception ex)
            {
                var exMessage = ex.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }

        [HttpGet("GetAllByUserId")]
        public async Task<ActionResult<OperationResult>> GetPromotionsByUserId()
        {
            try
            {
                var promotions = await _salePromotionService.GetAllByUserId();
                var promotionVMs = _mapper.Map<List<SalePromotionVM>>(promotions);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: promotionVMs);
            }
            catch (NullReferenceException nullEx)
            {
                return new OperationResult(false, nullEx.Message, StatusCodes.Status204NoContent);
            }
            catch (AutoMapperMappingException mapperEx)
            {
                return new OperationResult(false, mapperEx.Message, StatusCodes.Status422UnprocessableEntity);
            }
            catch (Exception ex)
            {
                var exMessage = ex.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }


        [HttpGet("GetAllAdminPromotions")]
        public async Task<ActionResult<OperationResult>> GetAllAdminPromotions()
        {
            try
            {
                var promotions = await _salePromotionService.GetAllAdminPromotions();
                var promotionVMs = _mapper.Map<List<SalePromotionVM>>(promotions);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: promotionVMs);
            }
            catch (NullReferenceException nullEx)
            {
                return new OperationResult(false, nullEx.Message, StatusCodes.Status204NoContent);
            }
            catch (UnauthorizedAccessException authEx)
            {
                return new OperationResult(false, authEx.Message, StatusCodes.Status403Forbidden);
            }
            catch (AutoMapperMappingException mapperEx)
            {
                return new OperationResult(false, mapperEx.Message, StatusCodes.Status422UnprocessableEntity);
            }
            catch (Exception ex)
            {
                var exMessage = ex.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }


        [HttpGet("GetByIdAsync/{id}")]
        public async Task<ActionResult<OperationResult>> GetByIdAsync(int id)
        {
            try
            {
                var promotion = await _salePromotionService.GetByIdAsync(id);
                var promotionVM = _mapper.Map<SalePromotionVM>(promotion);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: promotionVM);
            }
            catch (NullReferenceException nullEx)
            {
                return new OperationResult(false, nullEx.Message, StatusCodes.Status204NoContent);
            }
            catch (AutoMapperMappingException mapperEx)
            {
                return new OperationResult(false, mapperEx.Message, StatusCodes.Status422UnprocessableEntity);
            }
            catch (Exception ex)
            {
                var exMessage = ex.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }
        [HttpPost("AddAdminPromotion")]
        public async Task<ActionResult<OperationResult>> AddAsync([FromBody] SalePromotionDTO salePromotionDTO)
        {
            try
            {
                if (salePromotionDTO == null)
                {
                    return BadRequest("Promotion is null");
                }
                if (ModelState.IsValid)
                {
                    var promotion = _mapper.Map<Promotion>(salePromotionDTO);
                    await _salePromotionService.AddAdminPromotionAsync(promotion);
                    return new OperationResult(true, "Promotion add succesfully", StatusCodes.Status200OK);
                }
                return BadRequest("Sale Promotion data invalid");
            }
            catch (DbUpdateException dbEx)
            {
                return new OperationResult(false, dbEx.Message, StatusCodes.Status500InternalServerError);
            }
            catch (InvalidOperationException operationEx)
            {
                return new OperationResult(false, operationEx.Message, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, StatusCodes.Status400BadRequest);
            }
        }

        [HttpPost("AddUserPromotion/{postId}")]
        public async Task<ActionResult<OperationResult>> AddAsync([FromBody] SalePromotionDTO salePromotionDTO, int postId)
        {
            try
            {
                if (salePromotionDTO == null)
                {
                    return BadRequest("Promotion is null");
                }
                if (ModelState.IsValid)
                {
                    var promotion = _mapper.Map<Promotion>(salePromotionDTO);
                    await _salePromotionService.AddUserPromotionAsync(promotion,postId);
                    return new OperationResult(true, "Promotion add succesfully", StatusCodes.Status200OK);
                }
                return BadRequest("Sale Promotion data invalid");
            }
            catch (DbUpdateException dbEx)
            {
                return new OperationResult(false, dbEx.Message, StatusCodes.Status500InternalServerError);
            }
            catch (InvalidOperationException operationEx)
            {
                return new OperationResult(false, operationEx.Message, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, StatusCodes.Status400BadRequest);
            }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<OperationResult>> DeleteAsync(int id)
        {
            try
            {
                await _salePromotionService.DeletedByIdAsync(id);
                return new OperationResult(true, "Promotion deleted succesfully", StatusCodes.Status200OK);
            }
            catch (DbUpdateException dbEx)
            {
                return new OperationResult(false, dbEx.Message, StatusCodes.Status500InternalServerError);
            }
            catch (InvalidOperationException operationEx)
            {
                return new OperationResult(false, operationEx.Message, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, StatusCodes.Status400BadRequest);
            }
        }

        [HttpPost("Update/{id}")]
        public async Task<ActionResult<OperationResult>> UpdateAsync(int id, [FromBody] SalePromotionDTO salePromotionDTO)
        {
            try
            {
                if (salePromotionDTO == null || id != salePromotionDTO.Id)
                {
                    return BadRequest("Invalid request");
                }
                if (ModelState.IsValid)
                {
                    var promotion = _mapper.Map<Promotion>(salePromotionDTO);
                    await _salePromotionService.UpdateAsync(id, promotion);
                    return new OperationResult(true, "Promotion update succesfully", StatusCodes.Status200OK);
                }
                return BadRequest("Sale Promotion data invalid");
            }
            catch (DbUpdateException dbEx)
            {
                return new OperationResult(false, dbEx.Message, StatusCodes.Status500InternalServerError);
            }
            catch (InvalidOperationException operationEx)
            {
                return new OperationResult(false, operationEx.Message, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, StatusCodes.Status400BadRequest);
            }
        }
    }
}
