using GoWheels_WebAPI.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace GoWheels_WebAPI.Service
{
    public class StartupService : IStartupService   
    {
        private readonly IBookingService _bookingService;
        private readonly IPostService _postService;
        private readonly IPostPromotionService _postPromotionService;
        private readonly IUserPromotionService _userPromotionService;

        public StartupService(IBookingService bookingService,
                                IPostService postService,
                                IPostPromotionService postPromotionService,
                                IUserPromotionService userPromotionService)
        {
            _bookingService = bookingService;
            _postService = postService;
            _postPromotionService = postPromotionService;
            _userPromotionService = userPromotionService;
        }

        public void UpdateBookingsOnStartup()
        {
            try
            {
                _bookingService.UpdateBookingStatus();
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

        private void UpdatePostRideNumber()
        {
            var bookings = _bookingService.GetAllCompleteBooking();
            foreach (var booking in bookings)
            {

                if (!booking.IsRideCounted)
                {
                    _postService.UpdateRideNumber(booking.PostId, 1);
                    booking.IsRideCounted = true;
                    _bookingService.Update(booking.Id, booking);
                }
            }
        }

        private void UpdatePostPromotion()
        {
            var promotions = _userPromotionService.GetAllByUserRole();
            foreach (var promotion in promotions)
            {
                if (promotion.IsDeleted || promotion.ExpiredDate < DateTime.Now)
                {
                    var postPromotions = _postPromotionService.GetAllByPromotionId(promotion.Id);
                    _postPromotionService.DeletedRange(postPromotions);
                }
            }
        }

        public void UpdatePostOnStartup()
        {
            try
            {
                UpdatePostRideNumber();
                UpdatePostPromotion();

            }
            catch (InvalidOperationException operationEx)
            {
                Console.WriteLine("An error occurred: " + operationEx.Message);
                Console.WriteLine("Stack Trace: " + operationEx.StackTrace);
            }
            catch (NullReferenceException nullEx)
            {
                Console.WriteLine("An error occurred: " + nullEx.Message);
                Console.WriteLine("Stack Trace: " + nullEx.StackTrace);
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine("An error occurred: " + dbEx.Message);
                Console.WriteLine("Stack Trace: " + dbEx.StackTrace);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                Console.WriteLine("Stack Trace: " + ex.StackTrace);
            }
        }
    }
}
