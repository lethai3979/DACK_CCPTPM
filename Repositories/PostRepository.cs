using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _context;

        public PostRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddPostImagesAsync(List<string> postImageUrls, int postId)
        {
            foreach (var url in postImageUrls)
            {
                var postImage = new PostImage()
                {
                    PostId = postId,
                    Url = url
                };
                await _context.PostImages.AddAsync(postImage);
            }
            await _context.SaveChangesAsync();

        }

        public async Task DeletePostImagesAsync(int postId)
        {
            var imagesList = await _context.PostImages.Where(p => p.PostId == postId).ToListAsync();
            _context.PostImages.RemoveRange(imagesList);
        }

        public async Task AddAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

        }

        public async Task UpdateAsync(Post post)
        {
            var existingPost = _context.ChangeTracker.Entries<Post>().FirstOrDefault(e => e.Entity.Id == post.Id);
            if (existingPost != null)
            {
                _context.Entry(existingPost.Entity).State = EntityState.Detached;
            }
            _context.Entry(post).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            //detached tracking obj after modified
            _context.Entry(post).State = EntityState.Detached;
        }

        public async Task DeleteAsync(Post post)
        {
            _context.Entry(post).State = EntityState.Modified;
            post.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Post>> GetAllAsync()
            => await _context.Posts.AsNoTracking()
                                    .Include(p => p.CarType)
                                    .Include(p => p.Company)
                                    .Include(p => p.Images)
                                    .Include(p => p.Ratings.Where(r => !r.IsDeleted))
                                    .ThenInclude(r => r.User)
                                    .Include(p => p.User)
                                    .Include(p => p.PostAmenities)
                                    .ThenInclude(p => p.Amenity)
                                    .Include(p => p.PostPromotions)
                                    .ThenInclude(p => p.Promotion)
                                    .Where(p => !p.IsDeleted && !p.IsDisabled)
                                    .ToListAsync();


        public async Task<Post> GetByIdAsync(int id)
            => await _context.Posts.AsNoTracking()
                                    .Include(p => p.CarType)
                                    .Include(p => p.Company)
                                    .Include(p => p.Images)
                                    .Include(p => p.User)
                                    .Include(p => p.Ratings.Where(r => !r.IsDeleted))
                                    .Include(p => p.PostAmenities)
                                    .ThenInclude(p => p.Amenity)
                                    .Include(p => p.PostPromotions)
                                    .ThenInclude(p => p.Promotion)
                                    .Where(p => !p.IsDeleted && !p.IsDisabled)
                                    .FirstOrDefaultAsync(p => p.Id == id)
                                    ?? throw new NullReferenceException("Post not found");



        public async Task<List<Post>> GetPostsByUserIdAsync(string userId)
            => await _context.Posts.Include(p => p.CarType)
                            .Include(p => p.Company)
                            .Include(p => p.Images)
                            .Include(p => p.Ratings.Where(r => !r.IsDeleted))
                            .Include(p => p.User)
                            .Include(p => p.PostAmenities)
                            .ThenInclude(p => p.Amenity)
                            .Include(p => p.PostPromotions)
                            .ThenInclude(p => p.Promotion)
                            .Where(p => !p.IsDeleted && p.UserId == userId)
                            .ToListAsync();
    }
}
