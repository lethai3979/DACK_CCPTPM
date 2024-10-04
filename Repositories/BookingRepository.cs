using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

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
            => await _context.Bookings.AsNoTracking().Include(b => b.Post)
                                        .Include(b => b.User)
                                        .Include(b => b.Post)
                                        .Include(b => b.Promotion)
                                        .Where(b => !b.IsDeleted).ToListAsync();

        public async Task<List<Booking>> GetAllPersonalAsync(string userId)
           => await _context.Bookings.AsNoTracking().Include(b => b.Post)
                                        .Include(b => b.User)
                                        .Include(b => b.Post)
                                        .Include(b => b.Promotion)
                                        .Where(b => !b.IsDeleted && b.UserId == userId)
                                        .ToListAsync();

        public async Task<List<Booking>> GetAllCancelRequestAsync()
            => await _context.Bookings.AsNoTracking().Include(b => b.Post)
                                        .Include(b => b.User)
                                        .Include(b => b.Post)
                                        .Include(b => b.Promotion)
                                        .Where(b => !b.IsDeleted && b.IsRequest)
                                        .ToListAsync();

        public async Task<Booking?> GetByIdAsync(int id)
            => await _context.Bookings.AsNoTracking().Include(b => b.Post)
                                        .Include(b => b.User)
                                        .Include(b => b.Post)
                                        .Include(b => b.Promotion)
                                        .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

        public async Task UpdateAsync(Booking booking)
        {
            // Kiểm tra và detach đối tượng nếu đang bị theo dõi bởi DbContext
            var existingBooking = _context.ChangeTracker.Entries<Booking>().FirstOrDefault(e => e.Entity.Id == booking.Id);
            if (existingBooking != null)
            {
                _context.Entry(existingBooking.Entity).State = EntityState.Detached;
            }

            // Không cần gọi Attach nữa nếu bạn dùng Update trực tiếp
            _context.Bookings.Update(booking);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            // Detached sau khi update
            _context.Entry(booking).State = EntityState.Detached;
        }

    }
}
