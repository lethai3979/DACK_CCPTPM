using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Repositories.Interface
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task AddPostImagesAsync(List<string> postImageUrls, int postId);
        Task DeletePostImagesAsync(int postId);
        Task<List<Post>> GetPostsByUserIdAsync(string userId);

    }
}
