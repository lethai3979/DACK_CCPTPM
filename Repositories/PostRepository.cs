using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Repositories
{
    public class PostRepository : IGenericRepository<Post>
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
            }
            await _context.SaveChangesAsync();

        }

        public async Task AddAsync(Post post)
        {
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

        }

        public async Task DeleteAsync(Post post)
        {
            _context.Entry(post).State = EntityState.Modified;
            post.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Post>> GetAllAsync()
            => await _context.Posts.Include(p => p.CarType)
                                    .Include(p => p.Company)
                                    .Include(p => p.Images)
                                    .Include(p => p.PostAmenities)
                                    .ThenInclude(p => p.Amenity).ToListAsync();
                                    

        public async Task<Post?> GetByIdAsync(int id)
            => await _context.Posts.Include(p => p.CarType)
                                    .Include(p => p.Company)
                                    .Include(p => p.PostAmenities)
                                    .ThenInclude(p => p.Amenity).FirstOrDefaultAsync(p => p.Id == id);


        public async Task UpdateAsync(Post post)
        {
            var existingPost =  _context.ChangeTracker.Entries<Post>().FirstOrDefault(e => e.Entity.Id == post.Id);
            if(existingPost != null)
            {
                _context.Entry(existingPost.Entity).State = EntityState.Detached;
            }

            _context.Posts.Attach(post);  // Attach target modified obj to context 
            _context.Entry(post).State = EntityState.Modified;
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();

            //detached tracking obj after modified
            _context.Entry(post).State = EntityState.Detached;
        }
    }
}
