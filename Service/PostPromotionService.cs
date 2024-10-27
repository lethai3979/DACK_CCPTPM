using AutoMapper;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Service
{
    public class PostPromotionService
    {
        private readonly PostPromotionReposity _postPromotionRepository;
        private readonly PostService _postService;

        public PostPromotionService(PostPromotionReposity postPromotionRepository,
                                    PostService postService)
        {
            _postPromotionRepository = postPromotionRepository;
            _postService = postService;
        }

        public async Task<List<PostPromotion>> GetAllByPromotionIdAsync(int promotionId)
            => await _postPromotionRepository.GetAllByPromotionIdAsync(promotionId);

        public async Task AddAsync(Promotion promotion, int postId)
        {
            try
            {
                var postPromotion = new PostPromotion()
                {
                    PostId = postId,
                    PromotionId = promotion.Id
                };
                await _postPromotionRepository.AddAsync(postPromotion);
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

        public async Task AddRangeAsync(int promotionId, List<int> postIds)
        {
            try
            {
                foreach (var id in postIds)
                {
                    var postPromotion = new PostPromotion()
                    {
                        PostId = id,
                        PromotionId = promotionId
                    };
                    await _postPromotionRepository.AddAsync(postPromotion);
                }
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

        public async Task AddRangeAsync(int promotionId, List<Post> posts)
        {
            try
            {
                foreach (var post in posts)
                {
                    var postPromotion = new PostPromotion()
                    {
                        PostId = post.Id,
                        PromotionId = promotionId
                    };
                    await _postPromotionRepository.AddAsync(postPromotion);
                }
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


        public async Task DeletedAsync(Promotion promotion)
        {
            try
            {
                var postPromotion = await _postPromotionRepository.GetByPromotionIdAsync(promotion.Id);
                await _postPromotionRepository.DeleteAsync(postPromotion);
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

        public async Task DeletedRangeAsync(List<PostPromotion> postPromotions)
        {
            try
            {
                await _postPromotionRepository.DeleteRangeAsync(postPromotions);
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
