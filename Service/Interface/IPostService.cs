using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface IPostService
    {
        Task<List<Post>> GetAllAsync();
        Task<Post> GetByIdAsync(int id);
        Task<List<Post>> GetAllByUserId();
        Task<List<Post>> GetAllByUserId(string userId);
        Task AddAsync(Post post, IFormFile formFile, List<IFormFile> formFiles, List<int> amenitiesIds);
        Task UpdateAsync(int id, Post post, IFormFile image, List<int> amenitiesIds);
        Task UpdatePostImagesAsync(List<IFormFile> imageFiles, int postId);
        Task UpdatePostAverageRatingAsync(int postId, float avgRating);
        Task UpdateRideNumberAsync(int postId, int rideNumber);
        Task DeleteByIdAsync(int id);
        Task DisablePostByIdAsync(int id);
    }
}
