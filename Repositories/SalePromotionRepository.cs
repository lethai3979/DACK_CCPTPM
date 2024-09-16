using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Repositories
{
    public class SalePromotionRepository : IGenericRepository<Promotion>
    {
        public Task AddAsync(Promotion entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Promotion entity)
        {
            throw new NotImplementedException();
        }

        public Task<List<Promotion>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Promotion?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Promotion entity, Promotion newEntity)
        {
            throw new NotImplementedException();
        }
    }
}
