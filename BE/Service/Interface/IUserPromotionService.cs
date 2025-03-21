using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface IUserPromotionService
    {
        List<Promotion> GetAllByUserRole();
        List<Promotion> GetAllByUserId();
        Promotion GetById(int id);
        void Add(Promotion promotion, List<int> postIds);
        void Update(int id, Promotion promotion, List<int> postIds);
        void DeleteById(int promotionId);
    }
}
