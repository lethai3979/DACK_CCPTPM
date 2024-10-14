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
                return 2;
            }
            return 3;
        }
        public async Task<List<Promotion>> GetAllAsync()
        {
            var promotionlist = await _salepromotionRepository.GetAllAsync();
            if (promotionlist.IsNullOrEmpty())
            {
                throw new NullReferenceException("List is empty");
            }
            return promotionlist;
        }

        public async Task<List<Promotion>> GetAllByUserId()
        {
            var promoList = await _salepromotionRepository.GetPromotionsByUserIdAsync(_userId);
            if (promoList.IsNullOrEmpty())
            {
                throw new NullReferenceException("List is empty");
            }
            return promoList;
        }


        public async Task<List<Promotion>> GetAllAdminPromotions()
        {
            var userRole = _httpContextAccessor.HttpContext?.User?
            .FindFirstValue(ClaimTypes.Role) ?? "Unknown";
            if (userRole != "Admin")
            {
                throw new UnauthorizedAccessException("Access denied");
            }
            var promoList = await _salepromotionRepository.GetPromotionsByUserIdAsync(_userId);
            if (promoList.IsNullOrEmpty())
            {
                throw new NullReferenceException("List is empty");
            }
            return promoList;
        }

        public async Task AddAsync(Promotion promotion)
        {
            try
            {
                promotion.PromotionTypeId = DeterminePromotionType();
                promotion.CreatedById = _userId;
                promotion.CreatedOn = DateTime.Now;
                promotion.IsDeleted = false;
                await _salepromotionRepository.AddAsync(promotion);
            }
            catch (DbUpdateException dbEx)
            {
                throw new DbUpdateException(dbEx.InnerException!.Message);
            }
            catch (InvalidOperationException operationEx)
            {
                throw new InvalidOperationException(operationEx.InnerException!.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateAsync(int id, Promotion promotion)
        {
            try
            {

                var existingPromotion = await _salepromotionRepository.GetByIdAsync(id);
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
            }
            catch (NullReferenceException nullEx)
            {
                throw new NullReferenceException(nullEx.Message);
            }
            catch (DbUpdateException dbEx)
            {
                throw new DbUpdateException(dbEx.InnerException!.Message);
            }
            catch (InvalidOperationException operationEx)
            {
                throw new InvalidOperationException(operationEx.InnerException!.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeletedByIdAsync(int id)
        {
            try
            {
                var promotion = await _salepromotionRepository.GetByIdAsync(id);
                promotion.ModifiedById = _userId;
                promotion.ModifiedOn = DateTime.Now;
                promotion.IsDeleted = true;
                await _salepromotionRepository.UpdateAsync(promotion);
            }
            catch (NullReferenceException nullEx)
            {
                throw new NullReferenceException(nullEx.Message);
            }
            catch (DbUpdateException dbEx)
            {
                throw new DbUpdateException(dbEx.InnerException!.Message);
            }
            catch (InvalidOperationException operationEx)
            {
                throw new InvalidOperationException(operationEx.InnerException!.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Promotion> GetByIdAsync(int id)
            => await _salepromotionRepository.GetByIdAsync(id);
    }
}
