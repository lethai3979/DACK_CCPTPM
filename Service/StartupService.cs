using AutoMapper;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace GoWheels_WebAPI.Service
{
    public class StartupService
    {
        private readonly BookingService _bookingService;
        private readonly PostService _postService;
        private readonly PostPromotionService _postPromotionService;
        private readonly UserPromotionService _userPromotionService;
        private readonly IMapper _mapper;

        public StartupService(BookingService bookingService,
                                PostService postService,
                                PostPromotionService postPromotionService,
                                UserPromotionService userPromotionService,
                                IMapper mapper)
        {
            _bookingService = bookingService;
            _postService = postService;
            _postPromotionService = postPromotionService;
            _userPromotionService = userPromotionService;
            _mapper = mapper;
        }

        public async Task UpdateBookingsOnStartup()
        {
            try
            {
                await _bookingService.UpdateBookingStatus();
            }
            catch (InvalidOperationException operationEx)
            {
                Debug.WriteLine("An error occurred: " + operationEx.Message);
                Debug.WriteLine("Stack Trace: " + operationEx.StackTrace);
            }
            catch (NullReferenceException nullEx)
            {
                Debug.WriteLine("An error occurred: " + nullEx.Message);
                Debug.WriteLine("Stack Trace: " + nullEx.StackTrace);
            }
            catch (DbUpdateException dbEx)
            {
                Debug.WriteLine("An error occurred: " + dbEx.Message);
                Debug.WriteLine("Stack Trace: " + dbEx.StackTrace);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred: " + ex.Message);
                Debug.WriteLine("Stack Trace: " + ex.StackTrace);
            }
        }

        private async Task UpdatePostRideNumberAsync()
        {
            var bookings = await _bookingService.GetAllCompleteBookingAsync();
            foreach (var booking in bookings)
            {

                if (!booking.IsRideCounted)
                {
                    await _postService.UpdateRideNumberAsync(booking.PostId, 1);
                    booking.IsRideCounted = true;
                    await _bookingService.UpdateAsync(booking.Id, booking);
                }
            }
        }

        private async Task UpdatePostPromotionAsync()
        {
            var promotions = await _userPromotionService.GetAllByUserRoleAsync();
            foreach (var promotion in promotions)
            {
                if(promotion.IsDeleted || promotion.ExpiredDate < DateTime.Now)
                {
                    var postPromotions = await _postPromotionService.GetAllByPromotionIdAsync(promotion.Id);   
                    await _postPromotionService.DeletedRangeAsync(postPromotions);
                }    
            }
        }

        public async Task UpdatePostOnStartup()
        {
            try
            {
                await UpdatePostRideNumberAsync();
                await UpdatePostPromotionAsync();

            }
            catch (InvalidOperationException operationEx)
            {
                Debug.WriteLine("An error occurred: " + operationEx.Message);
                Debug.WriteLine("Stack Trace: " + operationEx.StackTrace);
            }
            catch (NullReferenceException nullEx)
            {
                Debug.WriteLine("An error occurred: " + nullEx.Message);
                Debug.WriteLine("Stack Trace: " + nullEx.StackTrace);
            }
            catch (DbUpdateException dbEx)
            {
                Debug.WriteLine("An error occurred: " + dbEx.Message);
                Debug.WriteLine("Stack Trace: " + dbEx.StackTrace);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("An error occurred: " + ex.Message);
                Debug.WriteLine("Stack Trace: " + ex.StackTrace);
            }
        }
    }
}
