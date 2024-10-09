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
            return await _context.Amentities.ToListAsync();
        }
        public async Task<Amenity> GetByIdAsync(int id)
        {
            return await _context.Amentities.FirstOrDefaultAsync(a => a.Id == id) 
                                                ?? throw new NullReferenceException("Amenity not found");
        }
        public async Task UpdateAsync(Amenity amenity)
        {
            var existingAmenity = _context.ChangeTracker.Entries<Amenity>()
                                                 .FirstOrDefault(e => e.Entity.Id == amenity.Id);

            //detached same id obj
            if (existingAmenity != null)
            {
                _context.Entry(existingAmenity.Entity).State = EntityState.Detached;
            }

            _context.Amentities.Attach(amenity);  // Attach target modified obj to context 
            _context.Amentities.Update(amenity);
            await _context.SaveChangesAsync();

            //detached tracking obj after modified
            _context.Entry(amenity).State = EntityState.Detached;
        }
    }
}
