using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Repositories.Interface
{
    public interface IPromotionRepository : IGenericRepository<Promotion>
    {
        Promotion GetUserPromotionById(int id, string userId);
        List<Promotion> GetAllAdminPromotions();
        List<Promotion> GetPromotionsByUserId(string userId);
        List<Promotion> GetAllAdminPromotionsByUserId(string userId);
        List<Promotion> GetAllUserPromotions();

    }
}
