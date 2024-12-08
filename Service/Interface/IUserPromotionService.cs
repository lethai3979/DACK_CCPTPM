using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface IUserPromotionService
    {
        Task<List<Promotion>> GetAllByUserRoleAsync();
        Task<List<Promotion>> GetAllByUserId();
        Task<Promotion> GetByIdAsync(int id);
        Task AddAsync(Promotion promotion, List<int> postIds);
        Task UpdateAsync(int id, Promotion promotion, List<int> postIds);
        Task DeleteByIdAsync(int promotionId);
    }
}
