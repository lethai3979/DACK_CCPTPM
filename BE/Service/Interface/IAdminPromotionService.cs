using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface IAdminPromotionService
    {
        List<Promotion> GetAll();
        List<Promotion> GetAllAdminPromotions();
        List<Promotion> GetAllAdminPromotionsByUserId();
        Promotion GetById(int id);
        void Add(Promotion promotion);
        void Update(int id, Promotion promotion);
        void DeletedById(int id);

    }
}
