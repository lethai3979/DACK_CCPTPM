using AutoMapper;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Repositories;
using GoWheels_WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GoWheels_WebAPI.Service
{
    public class BookingService
    {
        private readonly BookingRepository _bookingRepository;
        private readonly PostService _postService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _userId;


        public BookingService(BookingRepository bookingRepository, 
                                PostService postService,
                                IMapper mapper, 
                                IHttpContextAccessor httpContextAccessor)
        {
            _bookingRepository = bookingRepository;
            _postService = postService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor.HttpContext?.User?
                     .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
        }

        public  async Task<List<Booking>> GetAllUnRecieveBookingsByPostIdAsync(int postId)
        {
            var bookings = await _bookingRepository.GetAllUnRecieveBookingByPostIdAsync(postId);
            if (bookings.Count == 0)
            {
                throw new NullReferenceException("List is empty");
            }
            return bookings;
        }

        public async Task<List<Booking>> GetAllWaitingBookingsByPostIdAsync(int postId)
        {
            var bookings = await _bookingRepository.GetAllWaitingBookingByPostIdAsync(postId);
            if (bookings.Count == 0)
            {
                throw new NullReferenceException("List is empty");
            }
            return bookings;
        }

        public async Task<List<Booking>> GetPendingBookingsByUserIdAsync()
        {
            var bookings = await _bookingRepository.GetAllPendingBookingByUserIdAsync(_userId);
            if (bookings.Count == 0)
            {
                throw new NullReferenceException("List is empty");
            }
            return bookings;
        }

        public async Task<List<Booking>> GetAllAsync()
        {
            var bookings = await _bookingRepository.GetAllAsync();
            if (bookings.Count == 0)
            {
                throw new NullReferenceException("List is empty");
            }
            return bookings;
        }
        public async Task<List<Booking>> GetAllCancelRequestAsync()
        {
            var requestList = await _bookingRepository.GetAllCancelRequestAsync();
            if (requestList.Count == 0)
            {
                throw new NullReferenceException("No request");
            }
            return requestList;
        }

        public async Task<List<Booking>> GetPersonalBookingsAsync()
        {
            var bookings = await _bookingRepository.GetAllPersonalBookingsAsync(_userId);
            if (bookings.Count == 0)
            {
                throw new NullReferenceException("List is empty");
            }
            return bookings;
        }

        public async Task<Booking> GetByIdAsync(int id) 
            => await _bookingRepository.GetByIdAsync(id);

        public async Task AddAsync(Booking booking)
        {
            try
            {
                booking.CreatedById = _userId;
                booking.UserId = _userId;
                booking.CreatedOn = DateTime.Now;
                booking.Status = "Pending";
                booking.IsDeleted = false;
                booking.IsPay = false;
                booking.IsRequest = false;
                booking.IsResponse = false;
                await _bookingRepository.AddAsync(booking);
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

        public async Task UpdateAsync(int id, Booking booking)
        {
            try
            {
                var existingBooking = await _bookingRepository.GetByIdAsync(id);
                booking.CreatedById = existingBooking.CreatedById;
                booking.CreatedOn = existingBooking.CreatedOn;
                booking.ModifiedById = existingBooking.ModifiedById;
                booking.ModifiedOn = existingBooking.ModifiedOn;
                booking.PrePayment = existingBooking.PrePayment;
                booking.RecieveOn = existingBooking.RecieveOn;
                booking.ReturnOn = existingBooking.ReturnOn;
                booking.FinalValue = existingBooking.FinalValue;
                booking.Total = existingBooking.Total;
                booking.UserId = existingBooking.UserId;
                booking.User = existingBooking.User;
                booking.PostId = existingBooking.PostId;
                booking.Post = existingBooking.Post;
                booking.IsDeleted = existingBooking.IsDeleted;
                var isValueChange = EditHelper<Booking>.HasChanges(booking, existingBooking);
                EditHelper<Booking>.SetModifiedIfNecessary(booking, isValueChange, existingBooking, _userId);
                await _bookingRepository.UpdateAsync(booking);
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

        public async Task UpdateOwnerConfirmAsync(int id, bool isAccept)
        {
            try
            {
                var booking = await _bookingRepository.GetByIdAsync(id);
                booking.ModifiedById = _userId;
                booking.ModifiedOn = DateTime.Now;
                booking.Status = "Accept Booking";
                booking.OwnerConfirm = isAccept;
                await _bookingRepository.UpdateAsync(booking);
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

        public async Task UpdateBookingStatus()
        {
            try
            {
                var bookings = await _bookingRepository.GetAllAsync();
                if (bookings.Count == 0)
                {
                    return;
                }
                foreach (var booking in bookings)
                {
                    if (booking.IsPay && !booking.IsResponse && booking.RecieveOn <= DateTime.Now)
                    {
                        booking.Status = "Renting";
                    }
                    else if (booking.IsPay && !booking.IsResponse && booking.ReturnOn < DateTime.Now)
                    {
                        booking.Status = "Conplete";
                    }
                    await _bookingRepository.UpdateAsync(booking);
                }
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

        public async Task DeleteAsync(int id)
        {
            try
            {
                var booking = await _bookingRepository.GetByIdAsync(id);
                booking.IsDeleted = true;
                await _bookingRepository.UpdateAsync(booking);
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

        public async Task RequestCancelBookingAsync(int id)
        {
            try
            {
                var existingBooking = await _bookingRepository.GetByIdAsync(id);
                existingBooking.IsRequest = true;
                existingBooking.Status = "Processing";
                existingBooking.ModifiedById = _userId;
                existingBooking.ModifiedOn = DateTime.Now;
                await _bookingRepository.UpdateAsync(existingBooking);
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

        public async Task ExamineCancelBookingRequestAsync(Booking booking, bool isAccept)
        {
            try
            {
                if (isAccept)
                {
                    booking.Status = "Refunded";
                    booking.IsResponse = true;
                }
                else
                {
                    booking.Status = "Request denied";
                    booking.IsResponse = false;
                }
                booking.ModifiedById = _userId;
                booking.ModifiedOn = DateTime.Now;
                await _bookingRepository.UpdateAsync(booking);
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
        public async Task CancelReportedBookingsAsync(Booking booking)
        {
            try
            {
                booking.Status = booking.IsPay ? "Refunded" : "Canceled";
                booking.IsResponse = true;
                await _bookingRepository.UpdateAsync(booking);
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
