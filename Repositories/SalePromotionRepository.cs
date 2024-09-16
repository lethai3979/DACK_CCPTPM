using AutoMapper;
using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.Entities;
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
            => await _context.Promotions.AsNoTracking().Where(p => !p.IsDeleted).ToListAsync();

        public async Task<Promotion?> GetByIdAsync(int id) 
            => await _context.Promotions.AsNoTracking().Where(p => !p.IsDeleted).FirstOrDefaultAsync();

        public async Task UpdateAsync(Promotion promotion, Promotion newPromotion)
        {
            _context.Entry(promotion).State = EntityState.Modified;
            _mapper.Map(newPromotion, promotion);
            await _context.SaveChangesAsync();
        }
    }
}
