using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Repositories.Interface
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<List<Booking>> GetAllDriverRequireBookingsAsync();
        Task<List<Booking>> GetAllByDriverAsync(string userId);
        Task<List<Booking>> GetAllByPostIdAsync(int postId);
        Task<List<Booking>> GetAllPersonalBookingsAsync(string userId);
        Task<List<Booking>> GetAllCancelRequestAsync();
        Task<List<Booking>> GetAllUnRecieveBookingByPostIdAsync(int postId);
        Task<List<Booking>> GetAllWaitingBookingByPostIdAsync(int postId);
        Task<List<Booking>> GetAllPendingBookingByUserIdAsync(string userId);
        Task<List<Booking>> GetAllCompleteBookingsAsync();

    }
}
