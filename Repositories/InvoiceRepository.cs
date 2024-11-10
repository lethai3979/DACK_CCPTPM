using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Repositories
{
    public class InvoiceRepository : IGenericRepository<Invoice>
    {
        private readonly ApplicationDbContext _context;

        public InvoiceRepository(ApplicationDbContext context)
        {
            _context = context;
        }   

        public async Task<List<Invoice>> GetAllAsync()
            => await _context.Invoices.AsNoTracking()
                                        .Include(i => i.Booking)
                                        .ThenInclude(b => b.User)
                                        .Include(i => i.DriverBooking)
                                        .ThenInclude(i => i.Driver)
                                        .ThenInclude(i => i.User)
                                        .OrderByDescending(i => i.CreatedOn)
                                        .ToListAsync();

        public async Task<List<Invoice>> GetAllByUserIdAsync(string userId)
            => await _context.Invoices.AsNoTracking()
                                        .Include(i => i.Booking)
                                        .ThenInclude(b => b.User)
                                        .Include(i => i.DriverBooking)
                                        .ThenInclude(i => i.Driver)
                                        .ThenInclude(i => i.User)
                                        .Where(i => i.Booking.UserId == userId)
                                        .OrderByDescending(i => i.CreatedOn)
                                        .ToListAsync();

        public async Task<List<Invoice>> GetAllByDriverAsync(string userId)
            => await _context.Invoices.AsNoTracking()
                                        .Include(i => i.Booking)
                                        .ThenInclude(b => b.User)
                                        .Include(i => i.DriverBooking)
                                        .ThenInclude(i => i.Driver)
                                        .ThenInclude(i => i.User)
                                        .Where(i => i.DriverBooking.Driver.UserId == userId)
                                        .OrderByDescending(i => i.CreatedOn)
                                        .ToListAsync();

        public async Task<List<Invoice>> GetAllRefundInvoicesAsync()
            => await _context.Invoices.AsNoTracking()
                                        .Include(i => i.Booking)
                                        .Include(i => i.DriverBooking)
                                        .ThenInclude(i => i.Driver)
                                        .ThenInclude(i => i.User)
                                        .Where(i => i.RefundInvoice)
                                        .OrderByDescending(i => i.CreatedOn)
                                        .ToListAsync();

        public async Task<Invoice> GetByIdAsync(int id)
            => await _context.Invoices.AsNoTracking()
                                        .Include(i => i.Booking)
                                        .ThenInclude(b => b.User)
                                        .Include(i => i.DriverBooking)
                                        .ThenInclude(d => d.Driver)
                                        .ThenInclude(d => d.User)
                                        .FirstOrDefaultAsync(i => i.Id == id)
                                        ?? throw new NullReferenceException("Invoice not found");


        public async Task<Invoice> GetByBookingIdAsync(int bookingId)
            => await _context.Invoices.AsNoTracking()
                                        .Include(i => i.Booking)
                                        .ThenInclude(b => b.User)
                                        .Include(i => i.DriverBooking)
                                        .ThenInclude(d => d.Driver)
                                        .ThenInclude(d => d.User)
                                        .FirstOrDefaultAsync(i => i.BookingId == bookingId)
                                        ?? throw new NullReferenceException("Invoice not found");

        public async Task<Invoice> GetByDriverBookingIdAsync(int driverBooking)
            => await _context.Invoices.AsNoTracking()
                                        .Include(i => i.Booking)
                                        .ThenInclude(b => b.User)
                                        .Include(i => i.DriverBooking)
                                        .FirstOrDefaultAsync(i => i.DriverBookingId == driverBooking)
                                        ?? throw new NullReferenceException("Invoice not found");

        public async Task AddAsync(Invoice invoice)
        {
            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Invoice invoice)
        {
            var existingInvoice = _context.Invoices.Local.FirstOrDefault(b => b.Id == invoice.Id);

            if (existingInvoice != null)
            {
                _context.Entry(existingInvoice).State = EntityState.Detached;
            }

            // Gán lại trạng thái cho đối tượng là modified và lưu các thay đổi
            _context.Entry(invoice).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Invoice invoice)
        {
            _context.Entry(invoice).State = EntityState.Modified;
            invoice.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
        

    }
}
