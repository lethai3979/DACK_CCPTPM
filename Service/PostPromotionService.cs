using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories.Interface;
using GoWheels_WebAPI.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Service
{
    public class PostPromotionService : IPostPromotionService
    {
        private readonly IPostPromotionRepository _postPromotionRepository;

        public PostPromotionService(IPostPromotionRepository postPromotionRepository)
        {
            _postPromotionRepository = postPromotionRepository;
        }

        public async Task<List<PostPromotion>> GetAllByPromotionIdAsync(int promotionId)
            => await _postPromotionRepository.GetAllByPromotionIdAsync(promotionId);

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
