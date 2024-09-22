using AutoMapper;
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
        private readonly IMapper _mapper;
        public SalePromotionService(SalePromotionRepository repository, IHttpContextAccessor contextAccessor, IMapper mapper)
        {
            _salepromotionRepository = repository;
            _httpContextAccessor = contextAccessor;
            _mapper = mapper;
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
                var promotionListDTO = _mapper.Map<List<SalePromotionVM>>(promotionlist);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: promotionListDTO);
            }
            return new OperationResult(message: "List empty", statusCode: StatusCodes.Status204NoContent);
        }

        public async Task<OperationResult> GetAllByPromotionTypeAsync(int typeId)
        {
            var promolist = await _salepromotionRepository.GetAllByPromotionTypeAsync(typeId);
            if (!promolist.IsNullOrEmpty())
            {
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: promolist);
            }
            return new OperationResult(message: "List empty", statusCode: StatusCodes.Status204NoContent);
        }

        public async Task<OperationResult> AddAsync(SalePromotionVM salePromotionDto)
        {
            salePromotionDto.CreatedById = _httpContextAccessor.HttpContext?.User?
                                    .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
            salePromotionDto.CreatedOn = DateTime.Now;
            salePromotionDto.PromotionTypeId = DeterminePromotionType();
            try
            {
                var promotion = _mapper.Map<Promotion>(salePromotionDto);
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

        public async Task<OperationResult> DeletedByIdAsync(int id)
        {
            try
            {
                var carType = await _salepromotionRepository.GetByIdAsync(id);
                if (carType == null)
                {
                    return new OperationResult(false, "Sale Promotion not found", StatusCodes.Status404NotFound);
                }
                await _salepromotionRepository.DeleteAsync(carType);
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
                    var promotionDto = _mapper.Map<SalePromotionVM>(promotion);
                    return new OperationResult(true, "Promotion found", StatusCodes.Status200OK, promotionDto);
                }
                return new OperationResult(false, "Promotion not found", StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, $"An error occurred: {ex.Message}", StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<OperationResult> UpdateAsync(int id, SalePromotionVM promotionDTO)
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
                promotion.PromotionTypeId = DeterminePromotionType();
                var isValueChange = EditHelper<Promotion>.HasChanges(promotion,existingPromotion);
                EditHelper<Promotion>.SetModifiedIfNecessary(promotion,isValueChange,existingPromotion,"NewUserId");
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
    }
}
