using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface IAmenityService
    {
        Task<List<Amenity>> GetAllAsync();
        Task<Amenity> GetByIdAsync(int id);
        Task AddAsync(Amenity amenity, IFormFile formFile);
        Task DeletedByIdAsync(int id);
        Task UpdateAsync(int id, Amenity amenity, IFormFile formFile);

    }
}
