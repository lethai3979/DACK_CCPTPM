using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Repositories
{
    public class DriverBookingRepository : IDriverBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public DriverBookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<DriverBooking> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<DriverBooking> GetAllByUserId(string userId)
            => _context.DriverBookings.Include(d => d.Driver)
                                            .Include(d => d.Invoices)
                                            .ThenInclude(i => i.Booking)
                                            .Where(d => !d.IsDeleted && d.Driver.UserId == userId)
                                            .ToList();

        public DriverBooking GetById(int id)
            => _context.DriverBookings.Include(d => d.Driver)
                                            .Include(d => d.Invoices)
                                            .ThenInclude(i => i.Booking)
                                            .FirstOrDefault(d => !d.IsCancel && !d.IsDeleted && d.Id == id)
                                            ?? throw new NullReferenceException("Driver booking not found");


        public DriverBooking GetByBookingId(int id)
            => _context.DriverBookings.Include(d => d.Driver)
                                            .Include(d => d.Invoices)
                                            .ThenInclude(i => i.Booking)
                                            .FirstOrDefault(db => db.Invoices.Any(inv => inv.BookingId == id))
                                            ?? throw new NullReferenceException("Driver booking not found");

        public void Add(DriverBooking driverBooking)
        {
            _context.DriverBookings.Add(driverBooking);
            _context.SaveChanges();
        }

        public void Delete(DriverBooking driverBooking)
        {
            _context.Entry(driverBooking).State = EntityState.Modified;
            driverBooking.IsDeleted = true;
            _context.SaveChanges();
        }

        public void Update(DriverBooking driverBooking)
        {
            var existingDriverBooking = _context.ChangeTracker.Entries<DriverBooking>()
                                                 .FirstOrDefault(e => e.Entity.Id == driverBooking.Id);

            //detached same id obj
            if (existingDriverBooking != null)
            {
                _context.Entry(existingDriverBooking.Entity).State = EntityState.Detached;
            }

            _context.Entry(driverBooking).State = EntityState.Modified;
            _context.SaveChanges();

            //detached tracking obj after modified
            _context.Entry(driverBooking).State = EntityState.Detached;
        }
    }
}
