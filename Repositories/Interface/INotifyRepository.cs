using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Repositories.Interface
{
    public interface INotifyRepository : IGenericRepository<Notify>
    {
        Task<List<Notify>> GetAllByUserIdAsync(string userId);
    }
}
