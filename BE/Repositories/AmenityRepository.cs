using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Repositories
{
    public class AmenityRepository : IGenericRepository<Amenity>
    {
        public readonly ApplicationDbContext _context;
        public AmenityRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Add(Amenity entity)
        {
            _context.Amentities.Add(entity);
            _context.SaveChanges();
        }
        public void Delete(Amenity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            entity.IsDeleted = true;
            _context.SaveChanges();
        }

        public List<Amenity> GetAll()
            => _context.Amentities.ToList();
        public Amenity GetById(int id)
            => _context.Amentities.FirstOrDefault(a => a.Id == id)
                                                ?? throw new NullReferenceException("Amenity not found");
        public void Update(Amenity amenity)
        {
            var existingAmenity = _context.ChangeTracker.Entries<Amenity>()
                                                 .FirstOrDefault(e => e.Entity.Id == amenity.Id);

            //detached same id obj
            if (existingAmenity != null)
            {
                _context.Entry(existingAmenity.Entity).State = EntityState.Detached;
            }

            _context.Entry(amenity).State = EntityState.Modified;
            _context.SaveChanges();

            //detached tracking obj after modified
            _context.Entry(amenity).State = EntityState.Detached;
        }
    }
}
