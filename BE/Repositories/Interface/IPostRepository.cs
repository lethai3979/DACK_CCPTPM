using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Repositories.Interface
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        void AddPostImages(List<string> postImageUrls, int postId);
        void DeletePostImages(int postId);
        List<Post> GetPostsByUserId(string userId);

    }
}
