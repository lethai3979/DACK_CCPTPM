using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Repositories.Interface
{
    public interface IFavoriteRepository
    {
        Task AddAsync(Favorite post);
        Task UpdateAsync(Favorite favorite);
        Task<List<Favorite>> GetAllByUserIdAsync(string userId);
        Task<Favorite?> GetByPostIdAsync(int postId, string userId);
        Task<Favorite> GetByIdAsync(int id, string userId);
    }
}
