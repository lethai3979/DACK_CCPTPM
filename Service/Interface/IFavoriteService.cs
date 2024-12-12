using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface IFavoriteService
    {
        List<Favorite> GetAll();
        void Add(Favorite favorite);
        void Deleted(int id);

    }
}
