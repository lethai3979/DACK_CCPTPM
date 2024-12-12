using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface IRatingService
    {
        List<Rating> GetAllByPostId(int postId);
        List<Rating> GetAll();
        Rating GetById(int id);
        void Add(Rating rating);
        void DeleteById(int id);
        void Update(int id, Rating rating);

    }
}
