using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface INotifyService
    {
        List<Notify> GetAllByUserId();
        Notify GetById(int id);
        void Add(Notify notify);
        void MarkAsRead(int id);
        void Delete(int id);

    }
}
