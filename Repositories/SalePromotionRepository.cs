using AutoMapper;
using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Repositories
{
    public class SalePromotionRepository : IGenericRepository<Promotion>
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public SalePromotionRepository(ApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task AddAsync(Promotion promotion)
        {
            await _context.Promotions.AddAsync(promotion);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Promotion promotion)
        {
            _context.Entry(promotion).State = EntityState.Modified;
            promotion.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Promotion>> GetAllAsync()
            => await _context.Promotions.AsNoTracking().Where(p => !p.IsDeleted).Include(p => p.PromotionType).ToListAsync();

        public async Task<Promotion?> GetByIdAsync(int id) 
            => await _context.Promotions.AsNoTracking()
                                        .Where(p => !p.IsDeleted)
                                        .Include(p => p.PromotionType)
                                        .FirstOrDefaultAsync(p => p.Id == id);

        public async Task UpdateAsync(Promotion promotion, Promotion newPromotion)
        {
            _context.Entry(promotion).State = EntityState.Modified;
            _mapper.Map(newPromotion, promotion);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Promotion>> GetPromotionsByUserIdAsync(string userId)
                    => await _context.Promotions.AsNoTracking()
                                                .Where(p => !p.IsDeleted && p.CreatedById == userId)
                                                .Include(p => p.PromotionType)
                                                .ToListAsync();

        public async Task<List<Promotion>> GetAdminPromotionsAsync()
                    => await _context.Promotions.AsNoTracking()
                                                .Where(p => !p.IsDeleted && p.PromotionTypeId == 1)
                                                .Include(p => p.PromotionType)
                                                .ToListAsync();

        public async Task UpdateAsync(Promotion promotion)
        {
            var existingPromotion = await _context.Promotions.AsNoTracking()
                                      .FirstOrDefaultAsync(p => p.Id == promotion.Id);

            if (existingPromotion == null)
            {
                throw new KeyNotFoundException($"Promotion with ID {promotion.Id} not found.");
            }

            // Gán lại trạng thái cho đối tượng là modified và lưu các thay đổi
            _context.Entry(promotion).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
