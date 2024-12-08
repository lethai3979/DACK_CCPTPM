using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Service.Interface
{
    public interface IBookingService
    {
        Task<List<Booking>> GetAllUnRecieveBookingsByPostIdAsync(int postId);
        Task<List<DateTime>> GetBookedDateByPostIdsAsync(int postId);
        Task<List<Booking>> GetAllWaitingBookingsByPostIdAsync(int postId);
        Task<List<Booking>> GetAllDriverRequireBookingsAsync();
        Task<List<Booking>> GetAllPendingBookingsByUserIdAsync();
        Task<List<Booking>> GetAllAsync();
        Task<List<Booking>> GetAllCompleteBookingAsync();
        Task<List<Booking>> GetAllCancelRequestAsync();
        Task<List<Booking>> GetPersonalBookingsAsync();
        Task<List<Booking>> GetAllByDriverAsync();
        Task<List<Booking>> GetAllByLocation(string latitude, string longitude);
        Task<Booking> GetByIdAsync(int id);
        Task<bool> CheckBookingValue(BookingDTO bookingDTO, decimal promotionValue);
        Task AddAsync(Booking booking);
        Task UpdateAsync(int id, Booking booking);
        Task UpdateOwnerConfirmAsync(int id, bool isAccept);
        Task UpdateBookingStatus();
        Task DeleteAsync(int id);
        Task RequestCancelBookingAsync(int id);
        Task ExamineCancelBookingRequestAsync(Booking booking, bool isAccept);
        Task CancelReportedBookingsAsync(Booking booking);

    }
}
