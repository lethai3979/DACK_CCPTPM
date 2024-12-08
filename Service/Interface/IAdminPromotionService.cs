using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface IAdminPromotionService
    {
        Task<List<Promotion>> GetAllAsync();
        Task<List<Promotion>> GetAllAdminPromotionsAsync();
        Task<List<Promotion>> GetAllAdminPromotionsByUserIdAsync();
        Task<Promotion> GetByIdAsync(int id);
        Task AddAsync(Promotion promotion);
        Task UpdateAsync(int id, Promotion promotion);
        Task DeletedByIdAsync(int id);

    }
}
