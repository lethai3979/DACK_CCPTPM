using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface INotifyService
    {
        Task<List<Notify>> GetAllByUserIdAsync();
        Task<Notify> GetByIdAsync(int id);
        Task AddAsync(Notify notify);
        Task MarkAsReadAsync(int id);
        Task DeleteAsync(int id);

    }
}
