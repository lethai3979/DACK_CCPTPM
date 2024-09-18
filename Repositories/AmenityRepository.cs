using AutoMapper;
using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Repositories
{
    public class AmenityRepository : IGenericRepository<Amenity>
    {
        public readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public AmenityRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task AddAsync(Amenity entity)
        {
            await _context.Amentities.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Amenity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            entity.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Amenity>> GetAllAsync()
        {
            return await _context.Amentities.Where(x => x.IsDeleted == false).ToListAsync();
        }
        public async Task<Amenity?> GetByIdAsync(int id)
        {
            return await _context.Amentities.Where(x => x.IsDeleted == false).FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task UpdateAsync(Amenity newEntity)
        {
            //_context.Entry(entity).State = EntityState.Modified;
            //_mapper.Map(newEntity, entity);
            //await _context.SaveChangesAsync();
        }
    }
}
