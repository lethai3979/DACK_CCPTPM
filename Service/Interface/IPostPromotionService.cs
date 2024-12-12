using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface IPostPromotionService
    {
        List<PostPromotion> GetAllByPromotionId(int promotionId);
        void AddRange(int promotionId, List<int> postIds);
        void DeletedRange(List<PostPromotion> postPromotions);

    }
}
