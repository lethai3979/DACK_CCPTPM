using AutoMapper;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Repositories;
using GoWheels_WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace GoWheels_WebAPI.Service
{
    public class UserPromotionService
    {
        private readonly PromotionRepository _promotionRepository;
        private readonly PostPromotionService _postPromotionService;
        private readonly PostService _postService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _userId;
        private readonly IMapper _mapper;

        public UserPromotionService(PromotionRepository promotionRepository,
                                    PostPromotionService postPromotionService,
                                    PostService postService,
                                    IHttpContextAccessor httpContextAccessor,
                                    string userId,
                                    IMapper mapper)
        {
            _promotionRepository = promotionRepository;
            _postPromotionService = postPromotionService;
            _postService = postService;
            _httpContextAccessor = httpContextAccessor;
            _userId = userId;
            _mapper = mapper;
        }


        public async Task<List<Promotion>> GetAllByUserId()
        {
            var promoList = await _promotionRepository.GetPromotionsByUserIdAsync(_userId);
            if (promoList.IsNullOrEmpty())
            {
                throw new NullReferenceException("List is empty");
            }
            return promoList;
        }

        public async Task<Promotion> GetByIdAsync(int id)
            => await _promotionRepository.GetByIdAsync(id);
        public async Task<bool> CheckValidatePost(List<int> postIds)
        {
            foreach (var postId in postIds)
            {
                var post = await _postService.GetByIdAsync(postId);
                if (_userId != post.CreatedById)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task AddAsync(Promotion promotion, List<int> postIds)
        {
            try
            {
                if (postIds.Contains(0) || postIds.Count == 0)
                {
                    throw new InvalidOperationException("Invalid post Id");
                }
                var isValidatePosts = await CheckValidatePost(postIds);
                if (!isValidatePosts) throw new UnauthorizedAccessException("Unauthorize");    
                promotion.IsAdminPromotion = false;
                promotion.CreatedById = _userId;
                promotion.CreatedOn = DateTime.Now;
                promotion.IsDeleted = false;
                await _promotionRepository.AddAsync(promotion);
                await _postPromotionService.AddRangeAsync(promotion.Id, postIds);
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

        public async Task<bool> IsPostIdsChange(List<int> postIds, int promotionId)
        {
            var previousDetails = await _postPromotionService.GetAllByPromotionIdAsync(promotionId);
            var posts = await _postService.GetAllAsync();
            foreach (var post in posts)
            {
                bool previousChecked = previousDetails != null && previousDetails.Any(c => c.Post.Id.Equals(post.Id));
                bool currentChecked = postIds.Contains(post.Id);
                if (previousChecked != currentChecked)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task UpdatePostPromotionsAsync(int promotionId, List<int> postIds)
        {
            var postPromotions = await _postPromotionService.GetAllByPromotionIdAsync(promotionId);
            await _postPromotionService.DeletedRangeAsync(postPromotions);
            await _postPromotionService.AddRangeAsync(promotionId, postIds);
        }


        public async Task UpdateAsync(int id, Promotion promotion, List<int> postIds)
        {
            try
            {
                var existingPromotion = await _promotionRepository.GetByIdAsync(id);
                promotion.CreatedOn = existingPromotion.CreatedOn;
                promotion.CreatedById = existingPromotion.CreatedById;
                promotion.ModifiedById = existingPromotion.ModifiedById;
                promotion.ModifiedOn = existingPromotion.ModifiedOn;
                promotion.IsDeleted = existingPromotion.IsDeleted;
                promotion.IsAdminPromotion = existingPromotion.IsAdminPromotion;
                if(postIds.Contains(0))
                {
                    throw new InvalidOperationException("Invalid post Id");
                }
                var isPostIdsChange = await IsPostIdsChange(postIds, promotion.Id);
                if(isPostIdsChange)
                {
                    await UpdatePostPromotionsAsync(promotion.Id, postIds);
                    EditHelper<Promotion>.SetModifiedIfNecessary(promotion, true, existingPromotion, _userId);
                }   
                else
                {
                    var isValueChange = EditHelper<Promotion>.HasChanges(promotion, existingPromotion);
                    EditHelper<Promotion>.SetModifiedIfNecessary(promotion, isValueChange, existingPromotion, _userId);
                }    
                await _promotionRepository.UpdateAsync(promotion);
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


        public async Task DeleteByIdAsync(int promotionId)
        {
            try
            {
                var promotion = await _promotionRepository.GetByIdAsync(promotionId);
                promotion.ModifiedById = _userId;
                promotion.ModifiedOn = DateTime.Now;
                promotion.IsDeleted = true;
                await _promotionRepository.UpdateAsync(promotion);
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
