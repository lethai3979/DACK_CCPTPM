using AutoMapper;
using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace GoWheels_WebAPI.Repositories
{
    public class RatingRepository : IGenericRepository<Rating>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public RatingRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task AddAsync(Rating rating)
        {
            await _context.Ratings.AddAsync(rating);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Rating rating)
        {
            var existingPromotion = await _context.Ratings.AsNoTracking()
                                       .FirstOrDefaultAsync(p => p.Id == rating.Id);

            if (existingPromotion == null)
            {
                throw new KeyNotFoundException($"Rating with ID {rating.Id} not found.");
            }

            // Gán lại trạng thái cho đối tượng là modified và lưu các thay đổi
            _context.Entry(rating).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Rating rating)
        {
            _context.Entry(rating).State = EntityState.Modified;
            rating.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Rating>> GetAllAsync()
            => await _context.Ratings.AsNoTracking()
                                        .Include(r => r.User)
                                        .ToListAsync();

        public async Task<Rating> GetByIdAsync(int id)
            => await _context.Ratings.AsNoTracking()
                                        .Include(r => r.User)
                                        .FirstOrDefaultAsync(p => p.Id == id)
                                                ?? throw new NullReferenceException("Rating not found");
        public async Task<List<Rating>> GetAllByPostId(int postId)
            => await _context.Ratings.AsNoTracking().Where(p => !p.IsDeleted && p.PostId == postId).ToListAsync();
        public async Task<float> GetAveragePostRatingAsync(int postId)
            => await _context.Ratings.AsNoTracking()
                                        .Where(p => !p.IsDeleted && p.PostId == postId)
                                        .Select(r => r.Point)
                                        .AverageAsync();
    }
}