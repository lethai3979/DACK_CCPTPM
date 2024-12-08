using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Repositories.Interface
{
    public interface IDriverRepository : IGenericRepository<Driver>
    {
        Task<Driver> GetByUserIdAsync(string userId);
    }
}
