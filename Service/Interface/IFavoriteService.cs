using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface IFavoriteService
    {
        Task<List<Favorite>> GetAllAsync();
        Task AddAsync(Favorite favorite);
        Task DeletedAsync(int id);

    }
}
