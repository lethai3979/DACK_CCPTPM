using GoWheels_WebAPI.Models.GoogleRespone;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface ILocatorService
    {
        Task<DistanceMatrixRespone> GetDistanceAsync(List<(string userId, string location)> userLocations, string destination);
        Task<DistanceMatrixRespone> GetDistanceAsync(List<(int bookingId, string location)> bookingLocations, string destination);
    }
}
