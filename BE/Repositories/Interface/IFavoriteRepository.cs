using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Repositories.Interface
{
    public interface IFavoriteRepository
    {
        void Add(Favorite post);
        void Update(Favorite favorite);
        List<Favorite> GetAllByUserId(string userId);
        Favorite? GetByPostId(int postId, string userId);
        Favorite GetById(int id, string userId);
    }
}
