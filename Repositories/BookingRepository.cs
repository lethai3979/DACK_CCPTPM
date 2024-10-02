using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Repositories
{
    public class BookingRepository : IGenericRepository<Booking>
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Booking booking)
        {
            await _context.AddAsync(booking);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Booking booking)
        {
            _context.Entry(booking).State = EntityState.Modified;
            booking.IsDeleted = true;
            await _context.SaveChangesAsync();
        }

        public async Task<List<Booking>> GetAllAsync()
            => await _context.Bookings.Include(b => b.Post)
                                        .Include(b => b.User)
                                        .Include(b => b.Post)
                                        .Include(b => b.Promotion)
                                        .Where(b => !b.IsDeleted).ToListAsync();

        public async Task<List<Booking>> GetAllPersonalAsync(string userId)
           => await _context.Bookings.Include(b => b.Post)
                                        .Include(b => b.User)
                                        .Include(b => b.Post)
                                        .Include(b => b.Promotion)
                                        .Where(b => !b.IsDeleted && b.UserId == userId)
                                        .ToListAsync();

        public async Task<List<Booking>> GetAllCancelRequestAsync()
            => await _context.Bookings.Include(b => b.Post)
                                        .Include(b => b.User)
                                        .Include(b => b.Post)
                                        .Include(b => b.Promotion)
                                        .Where(b => !b.IsDeleted && b.IsRequest)
                                        .ToListAsync();

        public async Task<Booking?> GetByIdAsync(int id)
            => await _context.Bookings.Include(b => b.Post)
                                        .Include(b => b.User)
                                        .Include(b => b.Post)
                                        .Include(b => b.Promotion)
                                        .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

        public async Task UpdateAsync(Booking booking)
        {
            var existingBooking = await _context.Bookings.AsNoTracking()
                          .FirstOrDefaultAsync(p => p.Id == booking.Id);

            if (existingBooking == null)
            {
                throw new KeyNotFoundException($"Promotion with ID {booking.Id} not found.");
            }

            // Gán lại trạng thái cho đối tượng là modified và lưu các thay đổi
            _context.Entry(booking).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
