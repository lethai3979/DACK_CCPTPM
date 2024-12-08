using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Repositories
{
    public class PostAmenityRepository : IPostAmenityRepository
    {
        private readonly ApplicationDbContext _context;

        public PostAmenityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PostAmenity>> GetAmenityByPostIdAsync(int id)
            => await _context.PostAmenities.Include(p => p.Post)
                                            .Include(p => p.Amenity)
                                            .Where(p => p.PostId == id)
                                            .ToListAsync();  


        public async Task RemoveRangeAsync(int postId)
        {
            var amenitiesList = await _context.PostAmenities
                                                .Where(p => p.PostId ==  postId)
                                                .ToListAsync();
            _context.PostAmenities.RemoveRange(amenitiesList);
            await _context.SaveChangesAsync();
        }

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
