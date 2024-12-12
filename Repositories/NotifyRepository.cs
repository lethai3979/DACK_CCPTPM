using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Repositories
{
    public class NotifyRepository : INotifyRepository
    {
        private readonly ApplicationDbContext _context;

        public NotifyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Notify> GetAll()
        => _context.Notifications.Where(n => !n.IsDeleted).ToList();

        public List<Notify> GetAllByUserId(string userId)
            => _context.Notifications.Where(n => n.UserId == userId && !n.IsDeleted).ToList();

        public Notify GetById(int id)
            => _context.Notifications.Find(id) ?? throw new NullReferenceException("Notify not found");

        public void Add(Notify notify)
        {
            _context.Add(notify);
            _context.SaveChanges();
        }

        public void Delete(Notify notify)
        {
            var trackedNotiy = _context.ChangeTracker.Entries<Notify>()
                                                 .FirstOrDefault(e => e.Entity.Id == notify.Id);

            //detached same id obj
            if (trackedNotiy != null)
            {
                _context.Entry(trackedNotiy.Entity).State = EntityState.Detached;
            }

            _context.Entry(notify).State = EntityState.Modified;
            notify.IsDeleted = true;
            _context.SaveChanges();

            //detached tracking obj after modified
            _context.Entry(notify).State = EntityState.Detached;
        }


        public void Update(Notify notify)
        {
            var trackedNotiy = _context.ChangeTracker.Entries<Notify>()
                                     .FirstOrDefault(e => e.Entity.Id == notify.Id);

            //detached same id obj
            if (trackedNotiy != null)
            {
                _context.Entry(trackedNotiy.Entity).State = EntityState.Detached;
            }

            _context.Entry(notify).State = EntityState.Modified;
            _context.SaveChanges();

            //detached tracking obj after modified
            _context.Entry(notify).State = EntityState.Detached;
        }
    }
}
