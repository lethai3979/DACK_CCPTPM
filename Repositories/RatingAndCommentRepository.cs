using AutoMapper;
using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace GoWheels_WebAPI.Repositories
{
    public class RatingAndCommentRepository : IGenericRepository<Rating>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public RatingAndCommentRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task AddAsync(Rating rating)
        {
            await _context.Ratings.AddAsync(rating);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Rating rating)
        {
            _context.Entry(rating).State = EntityState.Modified;
            rating.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Rating>> GetAllAsync()
            => await _context.Ratings.AsNoTracking().Where(p => !p.IsDeleted).ToListAsync();

        public async Task<Rating?> GetByIdAsync(int id)
            => await _context.Ratings.AsNoTracking().Where(p => !p.IsDeleted).FirstOrDefaultAsync(p => p.Id == id);

        public async Task UpdateAsync(Rating rating)
        {
            var existingRating = _context.ChangeTracker.Entries<Promotion>()
                                    .FirstOrDefault(e => e.Entity.Id == rating.Id);

            if (existingRating != null)
            {
                _context.Entry(existingRating.Entity).State = EntityState.Detached;
            }
            _context.Ratings.Attach(rating);
            _context.Ratings.Update(rating);
            await _context.SaveChangesAsync();

            _context.Entry(rating).State = EntityState.Detached;
        }

        public async Task<List<Rating>> GetAllCommentFromPost(int postId)
            => await _context.Ratings.AsNoTracking().Where(p => !p.IsDeleted && p.PostId == postId).ToListAsync();
    }
}