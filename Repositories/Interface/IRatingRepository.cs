using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Repositories.Interface
{
    public interface IRatingRepository : IGenericRepository<Rating>
    {
        Task<List<Rating>> GetAllByPostId(int postId);
        Task<float> GetAveragePostRatingAsync(int postId);
    }
}
