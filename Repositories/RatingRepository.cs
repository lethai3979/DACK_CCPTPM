using AutoMapper;
using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly ApplicationDbContext _context;
        public RatingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Rating rating)
        {
            _context.Ratings.Add(rating);
            _context.SaveChanges();
        }

        public void Update(Rating rating)
        {
            var existingRating = _context.ChangeTracker.Entries<Promotion>()
                                                            .FirstOrDefault(e => e.Entity.Id == rating.Id);

            if (existingRating == null)
            {
                throw new KeyNotFoundException($"Rating with ID {rating.Id} not found.");
            }

            // Gán lại trạng thái cho đối tượng là modified và lưu các thay đổi
            _context.Entry(rating).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(Rating rating)
        {
            _context.Entry(rating).State = EntityState.Modified;
            rating.IsDeleted = true;
            _context.SaveChanges();
        }

        public List<Rating> GetAll()
            => _context.Ratings.AsNoTracking()
                                        .Include(r => r.User)
                                        .ToList();

        public Rating GetById(int id)
            => _context.Ratings.AsNoTracking()
                                        .Include(r => r.User)
                                        .FirstOrDefault(p => p.Id == id)
                                                ?? throw new NullReferenceException("Rating not found");
        public List<Rating> GetAllByPostId(int postId)
            => _context.Ratings.AsNoTracking()
                                        .Include(r => r.User)
                                        .Where(p => !p.IsDeleted && p.PostId == postId)
                                        .ToList();
        public float GetAveragePostRating(int postId)
            => _context.Ratings.AsNoTracking()
                                        .Where(p => !p.IsDeleted && p.PostId == postId)
                                        .Select(r => r.Point)
                                        .Average();
    }
}