using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Repositories.Interface
{
    public interface IPromotionRepository : IGenericRepository<Promotion>
    {
        Task<Promotion> GetUserPromotionByIdAsync(int id, string userId);
        Task<List<Promotion>> GetAllAdminPromotionsAsync();
        Task<List<Promotion>> GetPromotionsByUserIdAsync(string userId);
        Task<List<Promotion>> GetAllAdminPromotionsByUserIdAsync(string userId);
        Task<List<Promotion>> GetAllUserPromotionsAsync();

    }
}
