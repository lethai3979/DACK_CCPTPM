using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories.Interface;
using GoWheels_WebAPI.Service.Interface;
using GoWheels_WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GoWheels_WebAPI.Service
{
    public class UserPromotionService : IUserPromotionService
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly IPostPromotionService _postPromotionService;
        private readonly IPostService _postService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _userId;

        public UserPromotionService(IPromotionRepository promotionRepository,
                                    IPostPromotionService postPromotionService,
                                    IPostService postService,
                                    IHttpContextAccessor httpContextAccessor)
        {
            _promotionRepository = promotionRepository;
            _postPromotionService = postPromotionService;
            _postService = postService;
            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor.HttpContext?.User?
                        .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
        }

        public List<Promotion> GetAllByUserRole()
            => _promotionRepository.GetAllUserPromotions();

        public List<Promotion> GetAllByUserId()
            => _promotionRepository.GetPromotionsByUserId(_userId);

        public Promotion GetById(int id)
            => _promotionRepository.GetUserPromotionById(id, _userId);

        private bool CheckValidatePost(List<int> postIds)
        {
            foreach (var postId in postIds)
            {
                var post = _postService.GetById(postId);
                if (_userId != post.CreatedById)
                {
                    return false;
                }
            }
            return true;
        }

        public void Add(Promotion promotion, List<int> postIds)
        {
            try
            {
                if (postIds.Contains(0) || postIds.Count == 0)
                {
                    throw new InvalidOperationException("Invalid post Id");
                }
                var isValidatePosts = CheckValidatePost(postIds);
                if (!isValidatePosts) throw new UnauthorizedAccessException("Unauthorize");
                promotion.IsAdminPromotion = false;
                promotion.CreatedById = _userId;
                promotion.CreatedOn = DateTime.Now;
                promotion.IsDeleted = false;
                _promotionRepository.Add(promotion);
                _postPromotionService.AddRange(promotion.Id, postIds);
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

        private bool IsPostIdsChange(List<int> postIds, int promotionId)
        {
            var previousDetails = _postPromotionService.GetAllByPromotionId(promotionId);
            var posts = _postService.GetAll();
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

        private void UpdatePostPromotions(int promotionId, List<int> postIds)
        {
            var postPromotions = _postPromotionService.GetAllByPromotionId(promotionId);
            _postPromotionService.DeletedRange(postPromotions);
            _postPromotionService.AddRange(promotionId, postIds);
        }


        public void Update(int id, Promotion promotion, List<int> postIds)
        {
            try
            {
                var existingPromotion = _promotionRepository.GetById(id);
                promotion.CreatedOn = existingPromotion.CreatedOn;
                promotion.CreatedById = existingPromotion.CreatedById;
                promotion.ModifiedById = existingPromotion.ModifiedById;
                promotion.ModifiedOn = existingPromotion.ModifiedOn;
                promotion.IsDeleted = existingPromotion.IsDeleted;
                promotion.IsAdminPromotion = existingPromotion.IsAdminPromotion;
                if (postIds.Contains(0))
                {
                    throw new InvalidOperationException("Invalid post Id");
                }
                var isPostIdsChange = IsPostIdsChange(postIds, promotion.Id);
                if (isPostIdsChange)
                {
                    UpdatePostPromotions(promotion.Id, postIds);
                    EditHelper<Promotion>.SetModifiedIfNecessary(promotion, true, existingPromotion, _userId);
                }
                else
                {
                    var isValueChange = EditHelper<Promotion>.HasChanges(promotion, existingPromotion);
                    EditHelper<Promotion>.SetModifiedIfNecessary(promotion, isValueChange, existingPromotion, _userId);
                }
                _promotionRepository.Update(promotion);
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


        public void DeleteById(int promotionId)
        {
            try
            {
                var promotion = _promotionRepository.GetById(promotionId);
                promotion.ModifiedById = _userId;
                promotion.ModifiedOn = DateTime.Now;
                promotion.IsDeleted = true;
                _promotionRepository.Update(promotion);
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
