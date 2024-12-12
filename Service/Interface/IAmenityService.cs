using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface IAmenityService
    {
        List<Amenity> GetAll();
        Amenity GetById(int id);
        void Add(Amenity amenity, IFormFile formFile);
        void DeletedById(int id);
        void Update(int id, Amenity amenity, IFormFile formFile);

    }
}
