using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.Entities;
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
            => await _context.Invoices.AsNoTracking().Include(i => i.Booking).ToListAsync();

        public async Task<List<Invoice>> GetAllByUserIdAsync(string userId)
            => await _context.Invoices.AsNoTracking().Include(i => i.Booking).ToListAsync();
        public async Task<Invoice?> GetByIdAsync(int id)
            => await _context.Invoices.AsNoTracking().Include(i => i.Booking).FirstOrDefaultAsync(i => i.Id == id);

        public async Task AddAsync(Invoice invoice)
        {
            await _context.Invoices.AddAsync(invoice);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Invoice invoice)
        {
            var existingInvoice = await _context.Promotions.AsNoTracking()
                                                  .FirstOrDefaultAsync(p => p.Id == invoice.Id);

            if (existingInvoice == null)
            {
                throw new KeyNotFoundException($"Promotion with ID {invoice.Id} not found.");
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
