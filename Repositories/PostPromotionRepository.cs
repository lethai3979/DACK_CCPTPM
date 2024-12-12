using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Repositories
{
    public class PostPromotionRepository : IPostPromotionRepository
    {
        private readonly ApplicationDbContext _context;

        public PostPromotionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(PostPromotion postPromotion)
        {
            _context.Add(postPromotion);
            _context.SaveChanges();
        }

        public void Delete(PostPromotion postPromotion)
        {
            _context.Remove(postPromotion);
            _context.SaveChanges();
        }

        public void DeleteRange(List<PostPromotion> postPromotions)
        {
            _context.PostPromotions.RemoveRange(postPromotions);
            _context.SaveChanges();
        }

        public void Update(PostPromotion postPromotion)
        {
            var existingPostPromotion = _context.ChangeTracker.Entries<Amenity>()
                                                 .FirstOrDefault(e => e.Entity.Id == postPromotion.Id);

            //detached same id obj
            if (existingPostPromotion != null)
            {
                _context.Entry(existingPostPromotion.Entity).State = EntityState.Detached;
            }

            _context.Entry(postPromotion).State = EntityState.Modified;
            _context.SaveChanges();

            //detached tracking obj after modified
            _context.Entry(postPromotion).State = EntityState.Detached;
        }

        public List<PostPromotion> GetAll()
            => _context.PostPromotions.ToList();



        public List<PostPromotion> GetAllByPromotionId(int promotionId)
                => _context.PostPromotions.Include(p => p.Post)
                                                .Where(p => p.PromotionId == promotionId)
                                                .ToList();

        public List<PostPromotion> GetAllByPostId(int promotionId)
                => _context.PostPromotions.Include(p => p.Post)
                                                .Where(p => p.PostId == promotionId)
                                                .ToList();

        public PostPromotion GetById(int id)
            => _context.PostPromotions
                        .FirstOrDefault(p => p.Id == id)
                        ?? throw new NullReferenceException("Post promotionId not found");

        public PostPromotion GetByPromotionId(int id)
            => _context.PostPromotions
                        .FirstOrDefault(p => p.PromotionId == id)
                        ?? throw new NullReferenceException("Post promotionId not found");


    }
}
