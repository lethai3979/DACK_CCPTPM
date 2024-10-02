using AutoMapper;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Repositories;
using GoWheels_WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace GoWheels_WebAPI.Service
{
    public class SalePromotionService
    {
        private readonly SalePromotionRepository _salepromotionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _userId;
        private readonly IMapper _mapper;
        public SalePromotionService(SalePromotionRepository repository, IHttpContextAccessor contextAccessor, IMapper mapper)
        {
            _salepromotionRepository = repository;
            _httpContextAccessor = contextAccessor;
            _mapper = mapper;
            _userId = _httpContextAccessor.HttpContext?.User?
                        .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
        }
        private int DeterminePromotionType()
        {
            var user = _httpContextAccessor.HttpContext?.User!;
            if (user.IsInRole("Admin"))
            {
                return 1;
            }
            return 2;
        }
        public async Task<OperationResult> GetAllAsync()
        {
            var promotionlist = await _salepromotionRepository.GetAllAsync();
            if (!promotionlist.IsNullOrEmpty())
            {
                var promotionListVM = _mapper.Map<List<SalePromotionVM>>(promotionlist);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: promotionListVM);
            }
            return new OperationResult(false, message: "List empty", statusCode: StatusCodes.Status204NoContent);
        }

        public async Task<OperationResult> GetPromotionsByRole()
        {
            var userRole = _httpContextAccessor.HttpContext?.User?
                        .FindFirstValue(ClaimTypes.Role) ?? "Unknown";
            List<Promotion> promoList = new List<Promotion>();
            if(userRole == "Admin")
            {
                promoList = await _salepromotionRepository.GetAdminPromotionsAsync();
            }    
            else
            {
                promoList = await _salepromotionRepository.GetPromotionsByUserIdAsync(_userId);
            }
            if (!promoList.IsNullOrEmpty())
            {
                var promotionListVM = _mapper.Map<List<SalePromotionVM>>(promoList);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: promotionListVM);
            }
            return new OperationResult(false, message: "List empty", statusCode: StatusCodes.Status204NoContent);
        }

        public async Task<OperationResult> AddAsync(SalePromotionDTO salePromotionDto)
        {
            try
            {
                var promotion = _mapper.Map<Promotion>(salePromotionDto);
                promotion.PromotionTypeId = DeterminePromotionType();
                promotion.CreatedById = _userId;
                promotion.CreatedOn = DateTime.Now;
                promotion.IsDeleted = false;
                await _salepromotionRepository.AddAsync(promotion);
                return new OperationResult(true, "Promotion added succesfully", StatusCodes.Status200OK);
            }
            catch (DbUpdateException dbEx)
            {
                var dbExMessage = dbEx.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, dbExMessage, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                var exMessage = ex.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }

        public async Task<OperationResult> UpdateAsync(int id, SalePromotionDTO promotionDTO)
        {
            try
            {

                var existingPromotion = await _salepromotionRepository.GetByIdAsync(id);
                if (existingPromotion == null)
                {
                    return new OperationResult(true, "Promotion not found", StatusCodes.Status404NotFound);
                }
                var promotion = _mapper.Map<Promotion>(promotionDTO);
                promotion.CreatedOn = existingPromotion.CreatedOn;
                promotion.CreatedById = existingPromotion.CreatedById;
                promotion.ModifiedById = existingPromotion.ModifiedById;
                promotion.ModifiedOn = existingPromotion.ModifiedOn;
                promotion.PromotionTypeId = existingPromotion.PromotionTypeId;
                promotion.PromotionType = existingPromotion.PromotionType;
                promotion.IsDeleted = existingPromotion.IsDeleted;
                var isValueChange = EditHelper<Promotion>.HasChanges(promotion, existingPromotion);
                EditHelper<Promotion>.SetModifiedIfNecessary(promotion, isValueChange, existingPromotion, _userId);
                await _salepromotionRepository.UpdateAsync(promotion);
                return new OperationResult(true, "Promotion update succesfully", StatusCodes.Status200OK);
            }
            catch (DbUpdateException dbEx)
            {
                var dbExMessage = dbEx.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, dbExMessage, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                var exMessage = ex.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }

        public async Task<OperationResult> DeletedByIdAsync(int id)
        {
            try
            {
                var promotion = await _salepromotionRepository.GetByIdAsync(id);
                if (promotion == null)
                {
                    return new OperationResult(false, "Sale Promotion not found", StatusCodes.Status404NotFound);
                }
                promotion.ModifiedById = _userId;
                promotion.ModifiedOn = DateTime.Now;
                promotion.IsDeleted = true;
                await _salepromotionRepository.UpdateAsync(promotion);
                return new OperationResult(true, "Sale Promotion deleted succesfully", StatusCodes.Status200OK);
            }
            catch (DbUpdateException dbEx)
            {
                var dbExMessage = dbEx.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, dbExMessage, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                var exMessage = ex.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }
        public async Task<OperationResult> GetByIdAsync(int id)
        {
            try
            {
                var promotion = await _salepromotionRepository.GetByIdAsync(id);
                if (promotion != null)
                {
                    var promotionVM = _mapper.Map<SalePromotionVM>(promotion);
                    return new OperationResult(true, "Promotion found", StatusCodes.Status200OK, promotionVM);
                }
                return new OperationResult(false, "Promotion not found", StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, $"An error occurred: {ex.Message}", StatusCodes.Status500InternalServerError);
            }
        }

    }
}
