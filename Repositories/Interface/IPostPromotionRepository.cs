using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Repositories.Interface
{
    public interface IPostPromotionRepository : IGenericRepository<PostPromotion>
    {
        void DeleteRange(List<PostPromotion> postPromotions);
        List<PostPromotion> GetAllByPromotionId(int promotionId);
        List<PostPromotion> GetAllByPostId(int promotionId);
        PostPromotion GetByPromotionId(int id);

    }
}
