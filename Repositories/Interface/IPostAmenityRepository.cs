using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Repositories.Interface
{
    public interface IPostAmenityRepository
    {
        Task<List<PostAmenity>> GetAmenityByPostIdAsync(int id);
        Task RemoveRangeAsync(int postId);
        Task AddRangeAsync(List<int> amenitiesIDs, int postId);

    }
}
