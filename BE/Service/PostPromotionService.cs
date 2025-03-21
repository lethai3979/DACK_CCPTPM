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

        public List<PostPromotion> GetAllByPromotionId(int promotionId)
            => _postPromotionRepository.GetAllByPromotionId(promotionId);

        public void AddRange(int promotionId, List<int> postIds)
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
                    _postPromotionRepository.Add(postPromotion);
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

        public void DeletedRange(List<PostPromotion> postPromotions)
        {
            try
            {
                _postPromotionRepository.DeleteRange(postPromotions);
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
