using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Repositories.Interface
{
    public interface IRatingRepository : IGenericRepository<Rating>
    {
        List<Rating> GetAllByPostId(int postId);
        float GetAveragePostRating(int postId);
    }
}
