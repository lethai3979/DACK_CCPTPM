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

        public async Task<OperationResult> AddAsync(BookingDTO bookingDTO)
        {
            try
            {
                var booking = _mapper.Map<Booking>(bookingDTO);
                booking.CreatedById = _userId;
                booking.UserId = _userId;
                booking.CreatedOn = DateTime.Now;
                booking.Status = "Waiting";
                booking.IsDeleted = false;
                booking.IsPay = false;
                booking.IsRequest = false;
                booking.IsResponse = false;
                await _bookingRepository.AddAsync(booking);
                return new OperationResult(true, "Booking add succesfully", StatusCodes.Status200OK, booking.Id);
            }
            catch (DbUpdateException dbEx)
            {
                var dbExMessage = dbEx.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, dbExMessage, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                var exMessage = ex.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }

        public async Task<OperationResult> UpdateAsync(int id, BookingDTO bookingDTO)
        {
            try
            {
                var existingBooking = await _bookingRepository.GetByIdAsync(id);
                if (existingBooking == null) {
                    return new OperationResult(false, "Booking not found", StatusCodes.Status404NotFound);
                }
                var booking = _mapper.Map(bookingDTO, existingBooking);
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
                booking.PromotionId = existingBooking.PromotionId;
                booking.Promotion = existingBooking.Promotion;
                booking.IsDeleted = existingBooking.IsDeleted;
                var isValueChange = EditHelper<Booking>.HasChanges(booking, existingBooking);
                EditHelper<Booking>.SetModifiedIfNecessary(booking, isValueChange, existingBooking, _userId);
                await _bookingRepository.UpdateAsync(booking);
                return new OperationResult(true, "Booking update succesfully", StatusCodes.Status200OK);
            }
            catch (DbUpdateException dbEx)
            {
                var dbExMessage = dbEx.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, dbExMessage, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                var exMessage = ex.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }

        public async Task<OperationResult> UpdateBookingStatus()
        {
            try
            {
                var bookings = await _bookingRepository.GetAllAsync();
                if (bookings.Count == 0)
                {
                    return new OperationResult(true, "No booking to update", StatusCodes.Status200OK);
                }
                foreach (var booking in bookings)
                {
                    if (booking.IsPay && !booking.IsResponse && booking.RecieveOn > DateTime.Now)
                    {
                        booking.Status = "Renting";
                    }
                    else if (booking.IsPay && !booking.IsResponse && booking.ReturnOn > DateTime.Now)
                    {
                        booking.Status = "Conplete";
                    }
                    await _bookingRepository.UpdateAsync(booking);
                }
                return new OperationResult(true, "Update successfully", StatusCodes.Status200OK);
            }
            catch (DbUpdateException dbEx)
            {
                var dbExMessage = dbEx.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, dbExMessage, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                var exMessage = ex.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            try
            {
                var booking = await _bookingRepository.GetByIdAsync(id);
                if (booking == null)
                {
                    return new OperationResult(false, "Booking not found", StatusCodes.Status404NotFound);
                }
                booking.IsDeleted = true;
                await _bookingRepository.UpdateAsync(booking);
                return new OperationResult(true, "Booking remove succesfully", StatusCodes.Status200OK);
            }
            catch (DbUpdateException dbEx)
            {
                var dbExMessage = dbEx.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, dbExMessage, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                var exMessage = ex.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }

        public async Task<OperationResult> RequestCancelBookingAsync(int id)
        {
            try
            {
                var existingBooking = await _bookingRepository.GetByIdAsync(id);
                if (existingBooking == null)
                {
                    return new OperationResult(false, "Booking not found", StatusCodes.Status404NotFound);
                }
                existingBooking.IsRequest = true;
                existingBooking.Status = "Processing";
                existingBooking.ModifiedById = _userId;
                existingBooking.ModifiedOn = DateTime.Now;
                await _bookingRepository.UpdateAsync(existingBooking);
                return new OperationResult(true, "Cancel booking request sent successfully", StatusCodes.Status200OK);
            }
            catch (DbUpdateException dbEx)
            {
                var dbExMessage = dbEx.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, dbExMessage, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                var exMessage = ex.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }

        public async Task ExamineCancelBookingRequestAsync(Booking booking, bool isAccept)
        {
            if(isAccept)
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

        public async Task<OperationResult> GetAllCancelRequestAsync()
        {
            var requestList = await _bookingRepository.GetAllCancelRequestAsync();
            if(requestList.Count == 0)
            {
                return new OperationResult(false, "No request available", StatusCodes.Status404NotFound);
            }    
            var requestVMs = _mapper.Map<List<BookingVM>>(requestList);
            return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: requestVMs);
        }

        public async Task<OperationResult> GetByIdAsync(int id)
        {
            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking == null)
            {
                return new OperationResult(false, "Booking not found", StatusCodes.Status404NotFound);
            }
            var bookingVM = _mapper.Map<BookingVM>(booking);
            return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: bookingVM);
        }

        public async Task<List<Booking>> GetAllWaitingBookingAsync(int postId)
            => await _bookingRepository.GetAllWaitingBookingAsync(postId);   
        

        public async Task<OperationResult> GetAllAsync()
        {
            var bookings = await _bookingRepository.GetAllAsync();
            if (bookings.Count == 0)
            {
                return new OperationResult(false, "No booking available", StatusCodes.Status404NotFound);
            }
            var bookingVMs = _mapper.Map<List<BookingVM>>(bookings);
            return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: bookingVMs);
        }
        public async Task<OperationResult> GetPersonalBookingAsync()
        {
            var bookings = await _bookingRepository.GetAllPersonalAsync(_userId);
            if (bookings.Count == 0)
            {
                return new OperationResult(false, "No booking available", StatusCodes.Status404NotFound);
            }
            var bookingVMs = _mapper.Map<List<BookingVM>>(bookings);
            return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: bookingVMs);
        }
    }
}
