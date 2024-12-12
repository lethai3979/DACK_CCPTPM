using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Repositories.Interface
{
    public interface INotifyRepository : IGenericRepository<Notify>
    {
        List<Notify> GetAllByUserId(string userId);
    }
}
