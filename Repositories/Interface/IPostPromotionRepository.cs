using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Repositories.Interface
{
    public interface IPostPromotionRepository : IGenericRepository<PostPromotion>
    {
        Task DeleteRangeAsync(List<PostPromotion> postPromotions);
        Task<List<PostPromotion>> GetAllByPromotionIdAsync(int promotionId);
        Task<List<PostPromotion>> GetAllByPostIdAsync(int promotionId);
        Task<PostPromotion> GetByPromotionIdAsync(int id);

    }
}
