using GoWheels_WebAPI.Utilities;

namespace GoWheels_WebAPI.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity, T newEntity);
        Task DeleteAsync(T entity);
    }
}
