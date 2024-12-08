using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Repositories
{
    public class FavoriteRepository : IFavoriteRepository
    {
        private readonly ApplicationDbContext _context;

        public FavoriteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Favorite post)
        {
            await _context.Favorites.AddAsync(post);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Favorite favorite)
        {
            _context.Entry(favorite).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Favorite>> GetAllByUserIdAsync(string userId)
            => await _context.Favorites.AsNoTracking()
                                        .Where(p => p.UserId == userId && !p.IsDeleted)
                                        .Include(p => p.Post)
                                        .ThenInclude(post => post.PostAmenities)
                                        .Include(p => p.Post)
                                        .ThenInclude(post => post.Images)
                                        .Include(p => p.Post)
                                        .ThenInclude(post => post.CarType)
                                        .Include(p => p.Post)
                                        .ThenInclude(post => post.Company)
                                        .ToListAsync();

        public async Task<Favorite?> GetByPostIdAsync(int postId, string userId)
            => await _context.Favorites.AsNoTracking()
                                        .Where(p => p.UserId == userId)
                                        .Include(p => p.Post)
                                        .ThenInclude(post => post.PostAmenities)
                                        .Include(p => p.Post)
                                        .ThenInclude(post => post.Images)
                                        .Include(p => p.Post)
                                        .ThenInclude(post => post.CarType)
                                        .Include(p => p.Post)
                                        .ThenInclude(post => post.Company)
                                        .FirstOrDefaultAsync(p => p.PostId == postId);
        
        public async Task<Favorite> GetByIdAsync(int id, string userId)
            => await _context.Favorites.AsNoTracking()
                                        .Where(p => p.UserId == userId)
                                        .FirstOrDefaultAsync(p => p.Id == id)
                                        ?? throw new NullReferenceException("Favorite not found");
    }
}
