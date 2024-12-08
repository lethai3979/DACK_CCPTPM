using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface IRatingService
    {
        Task<List<Rating>> GetAllByPostId(int postId);
        Task<List<Rating>> GetAllAsync();
        Task<Rating> GetByIdAsync(int id);
        Task AddAsync(Rating rating);
        Task DeleteByIdAsync(int id);
        Task UpdateAsync(int id, Rating rating);

    }
}
