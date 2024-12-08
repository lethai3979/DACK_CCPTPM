using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories.Interface;
using GoWheels_WebAPI.Service.Interface;
using GoWheels_WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GoWheels_WebAPI.Service
{
    public class AdminPromotionService : IAdminPromotionService
    {
        private readonly IPromotionRepository _salepromotionRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _userId;
        public AdminPromotionService(IPromotionRepository salepromotionRepository,
                                    IHttpContextAccessor contextAccessor)
        {
            _salepromotionRepository = salepromotionRepository;
            _httpContextAccessor = contextAccessor;
            _userId = _httpContextAccessor.HttpContext?.User?
                        .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
        }
        private bool IsAdminRole()
        {
            var user = _httpContextAccessor.HttpContext?.User!;
            if (user.IsInRole("Admin"))
            {
                return true;
            }
            return false;
        }
        public async Task<List<Promotion>> GetAllAsync()
            => await _salepromotionRepository.GetAllAsync();


        public async Task<List<Promotion>> GetAllAdminPromotionsAsync()
            => await _salepromotionRepository.GetAllAdminPromotionsAsync();

        public async Task<List<Promotion>> GetAllAdminPromotionsByUserIdAsync()
            => await _salepromotionRepository.GetAllAdminPromotionsByUserIdAsync(_userId);

        public async Task<Promotion> GetByIdAsync(int id)
            => await _salepromotionRepository.GetByIdAsync(id);



        public async Task AddAsync(Promotion promotion)
        {
            try
            {
                var isAdmin = IsAdminRole(); 
                promotion.IsAdminPromotion = true;
                promotion.CreatedById = _userId;
                promotion.CreatedOn = DateTime.Now;
                promotion.IsDeleted = false;
                await _salepromotionRepository.AddAsync(promotion);
            }
            catch(NullReferenceException nullEx)
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

        public async Task UpdateAsync(int id, Promotion promotion)
        {
            try
            {

                var existingPromotion = await _salepromotionRepository.GetByIdAsync(id);
                promotion.CreatedOn = existingPromotion.CreatedOn;
                promotion.CreatedById = existingPromotion.CreatedById;
                promotion.ModifiedById = existingPromotion.ModifiedById;
                promotion.ModifiedOn = existingPromotion.ModifiedOn;
                promotion.PostPromotions = existingPromotion.PostPromotions;
                promotion.IsDeleted = existingPromotion.IsDeleted;
                promotion.IsAdminPromotion = existingPromotion.IsAdminPromotion;
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


    }
}
