using AutoMapper;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Service
{
    public class StartupService
    {
        private readonly BookingService _bookingService;
        private readonly PostService _postService;
        private readonly IMapper _mapper;

        public StartupService(BookingService bookingService, PostService postService, IMapper mapper)
        {
            _bookingService = bookingService;
            _postService = postService;
            _mapper = mapper;
        }

        public async Task UpdateBookingsOnStartup()
        {
            try
            {
                await _bookingService.UpdateBookingStatus();
            }
            catch(NullReferenceException nullEx)
            {
                throw new NullReferenceException(nullEx.Message);
            }
            catch (DbUpdateException dbEx)
            {
                throw new DbUpdateException(dbEx.InnerException!.Message);
            }
            catch (InvalidOperationException operationEx)
            {
                throw new InvalidOperationException(operationEx.InnerException!.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdatePostOnStartup()
        {
            try
            {
                var bookings = await _bookingService.GetAllAsync();
                await _postService.UpdateRideNumberAsync(bookings);

            }
            catch (NullReferenceException)
            {
                return;
            }
            catch (DbUpdateException dbEx)
            {
                throw new DbUpdateException(dbEx.InnerException!.Message);
            }
            catch (InvalidOperationException operationEx)
            {
                throw new InvalidOperationException(operationEx.InnerException!.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
