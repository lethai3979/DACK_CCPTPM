using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Repositories
{
    public class PostAmenityRepository
    {
        private readonly ApplicationDbContext _context;

        public PostAmenityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PostAmenity>> GetAll()
            => await _context.PostAmenities.Include(p => p.Post)
                                            .Include(p => p.Amenity)
                                            .ToListAsync();  
        public async Task AddRangeAsync(List<int> amenitiesIDs, int postId)
        {
            foreach (int id in amenitiesIDs)
            {
                var postAmenity = new PostAmenity();
                postAmenity.AmenityId = id;
                postAmenity.PostId = postId;
                await _context.AddAsync(postAmenity);
            }
            await _context.SaveChangesAsync();
        }
    }
}
