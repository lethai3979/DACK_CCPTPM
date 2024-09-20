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
                                    .Include(p => p.PostAmenities)
                                    .ThenInclude(p => p.Amenity).ToListAsync();
                                    

        public async Task<Post?> GetByIdAsync(int id)
            => await _context.Posts.Include(p => p.CarType)
                                    .Include(p => p.Company)
                                    .Include(p => p.PostAmenities)
                                    .ThenInclude(p => p.Amenity).FirstOrDefaultAsync(p => p.Id == id);


        public Task UpdateAsync(Post entity)
        {
            throw new NotImplementedException();
        }
    }
}
