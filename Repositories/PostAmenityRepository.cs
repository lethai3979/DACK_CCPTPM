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

        public List<PostAmenity> GetAmenityByPostId(int id)
            => _context.PostAmenities.Include(p => p.Post)
                                            .Include(p => p.Amenity)
                                            .Where(p => p.PostId == id)
                                            .ToList();


        public void RemoveRange(int postId)
        {
            var amenitiesList = _context.PostAmenities
                                            .Where(p => p.PostId == postId)
                                            .ToList();
            _context.PostAmenities.RemoveRange(amenitiesList);
            _context.SaveChanges();
        }

        public void AddRange(List<int> amenitiesIDs, int postId)
        {
            foreach (int id in amenitiesIDs)
            {
                var postAmenity = new PostAmenity();
                postAmenity.AmenityId = id;
                postAmenity.PostId = postId;
                _context.Add(postAmenity);
            }
            _context.SaveChanges();
        }
    }
}
