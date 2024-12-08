using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface IPostPromotionService
    {
        Task<List<PostPromotion>> GetAllByPromotionIdAsync(int promotionId);
        Task AddRangeAsync(int promotionId, List<int> postIds);
        Task DeletedRangeAsync(List<PostPromotion> postPromotions);

    }
}
