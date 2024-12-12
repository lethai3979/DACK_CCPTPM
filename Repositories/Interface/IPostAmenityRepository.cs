using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Repositories.Interface
{
    public interface IPostAmenityRepository
    {
        List<PostAmenity> GetAmenityByPostId(int id);
        void RemoveRange(int postId);
        void AddRange(List<int> amenitiesIDs, int postId);

    }
}
