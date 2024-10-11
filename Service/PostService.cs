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
    public class PostService
    {

        private readonly PostRepository _postRepository;
        private readonly PostAmenityRepository _postAmenityRepository;
        private readonly AmenityService _amenityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private  readonly string _userId;

        public PostService(PostRepository postRepository,
                            PostAmenityRepository postAmenityRepository,
                            AmenityService amenityService,
                            IHttpContextAccessor httpContextAccessor,
                            IMapper mapper)
        {
            _postRepository = postRepository;
            _postAmenityRepository = postAmenityRepository;
            _amenityService = amenityService;
            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor.HttpContext?.User?
                        .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
        }

        public async Task<List<Post>> GetAllAsync()
        {
            var posts = await _postRepository.GetAllAsync();
            if (posts.Count == 0)
            {
                throw new NullReferenceException("List is empty");
            }
            return posts;
        }

        public async Task<Post> GetByIdAsync(int id)
            => await _postRepository.GetByIdAsync(id);

        public async Task<List<Post>> GetAllByUserId()
        {
            var posts = await _postRepository.GetPostsByUserIdAsync(_userId);
            if (posts.Count == 0)
            {
                throw new NullReferenceException("List is empty");
            }
            return posts;
        }

        public async Task AddAsync(Post post, List<string> imageUrls, List<int> amenitiesIds)
        {

            try
            {
                post.CreatedById = _userId;
                post.CreatedOn = DateTime.Now;
                post.UserId = _userId;
                post.AvgRating = 0;
                post.IsDeleted = false;
                post.IsDisabled = false;
                post.IsHidden = false;  
                post.IsAvailable = true;
                await _postRepository.AddAsync(post);
                if(imageUrls.Count != 0)
                {
                    await _postRepository.AddPostImagesAsync(imageUrls, post.Id);
                }
                if(amenitiesIds.Count != 0)
                {
                    await _postAmenityRepository.AddRangeAsync(amenitiesIds, post.Id);
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


        private async Task<bool> IsPostAmenityChange(List<int> selectedAmenities, int existingPostId)
        {
            var previousDetails = await _postAmenityRepository.GetAmenityByPostIdAsync(existingPostId);
            var amenities = await _amenityService.GetAllAsync();
            foreach (var amenity in amenities)
            {
                bool previousChecked = previousDetails != null && previousDetails.Any(c => c.AmenityId.Equals(amenity.Id));
                bool currentChecked = selectedAmenities.Contains(amenity.Id);
                if (previousChecked != currentChecked)
                {
                    return true;
                }
            }
            return false;
        }

        private async Task UpdatePostAmenitiesAsync(int postId, List<int> amenitiesIds)
        {
            await _postAmenityRepository.RemoveRangeAsync(postId);
            await _postAmenityRepository.AddRangeAsync(amenitiesIds, postId);
        }

        public async Task UpdateAsync(int id, Post post, List<int> amenitiesIds)
        {
            try
            {
                var existingPost = await _postRepository.GetByIdAsync(id);
                if(_userId != existingPost.UserId)
                {
                    throw new UnauthorizedAccessException("Unauthorize");
                }
                post.CreatedOn = existingPost.CreatedOn;
                post.CreatedById = existingPost.CreatedById;
                post.ModifiedById = existingPost.ModifiedById;
                post.ModifiedOn = existingPost.ModifiedOn;
                post.AvgRating = existingPost.AvgRating;
                post.IsAvailable = existingPost.IsAvailable;
                post.IsDeleted = existingPost.IsDeleted;
                post.Favorites = existingPost.Favorites;
                post.UserId = existingPost.UserId;
                post.User = existingPost.User;
                post.Ratings = existingPost.Ratings;
                post.Reports = existingPost.Reports;
                if(amenitiesIds.Contains(0) || amenitiesIds.Count == 0)
                {
                    amenitiesIds.Clear();
                }
                var isPostAmenitiesChange = await IsPostAmenityChange(amenitiesIds, existingPost.Id);
                if(isPostAmenitiesChange)
                {
                    await UpdatePostAmenitiesAsync(existingPost.Id, amenitiesIds);
                    EditHelper<Post>.SetModifiedIfNecessary(post, true, existingPost, _userId);
                }    
                else
                {
                    var isPostDataChange = EditHelper<Post>.HasChanges(post, existingPost);
                    EditHelper<Post>.SetModifiedIfNecessary(post, isPostDataChange, existingPost, _userId);
                }
                await _postRepository.UpdateAsync(post);
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

        public async Task UpdatePostImagesAsync(List<string> imageUrl, int postId)
        {
            try
            {
                var post = await _postRepository.GetByIdAsync(postId);
                if (_userId != post.UserId)
                {
                    throw new UnauthorizedAccessException("Unauthorize");
                }
                post.ModifiedById = _userId;
                post.ModifiedOn = DateTime.Now;
                await _postRepository.UpdateAsync(post);
                await _postRepository.DeletePostImagesAsync(post.Id);
                await _postRepository.AddPostImagesAsync(imageUrl, postId);
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

        public async Task UpdatePostAverageRatingAsync(int postId, float avgRating)
        {
            try
            {
                var post = await _postRepository.GetByIdAsync(postId);
                if (post == null) throw new InvalidOperationException("Post not found");
                post.AvgRating = avgRating;
                await _postRepository.UpdateAsync(post);
            }
            catch (DbUpdateException dbEx)
            {
                var dbExMessage = dbEx.InnerException?.Message ?? "An error occurred while updating the database.";
                throw new DbUpdateException(dbExMessage);
            }
            catch (Exception ex)
            {
                var exMessage = ex.InnerException?.Message ?? "An error occurred while updating the database.";
                throw new InvalidOperationException(exMessage);
            }
        }

        public async Task UpdateRideNumberAsync(List<Booking> bookings)
        {
            try
            {
                foreach (var booking in bookings)
                {
                    if(booking.RecieveOn >= DateTime.Now && booking.IsPay && !booking.IsResponse)
                    {
                        var post = await _postRepository.GetByIdWithoutConditionAsync(booking.Id);
                        if (post == null) throw new InvalidOperationException("Post not found");
                        post.RideNumber ++;
                        await _postRepository.UpdateAsync(post);
                    }                       
                }

            }
            catch (DbUpdateException dbEx)
            {
                var dbExMessage = dbEx.InnerException?.Message ?? "An error occurred while updating the database.";
                throw new DbUpdateException(dbExMessage);
            }
            catch (Exception ex)
            {
                var exMessage = ex.InnerException?.Message ?? "An error occurred while updating the database.";
                throw new InvalidOperationException(exMessage);
            }

        }


        public async Task DeleteByIdAsync(int id)
        {
            try
            {
                var post = await _postRepository.GetByIdAsync(id);
                if (_userId != post.UserId)
                {
                    throw new UnauthorizedAccessException("Unauthorize");
                }
                post.IsDeleted = true;
                post.ModifiedById = _userId;
                post.ModifiedOn = DateTime.Now; 
                await _postRepository.UpdateAsync(post);
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




        public async Task UpdateRideNumberAndIsAvailableAsync(int postId, bool isAvailable, int rideNumber)
        {
            var post = await _postRepository.GetByIdAsync(postId);
            if (post != null)
            {
                post.IsAvailable = isAvailable;
                if (post.RideNumber == 0 && rideNumber < 0)
                {
                    post.RideNumber = 0;
                }
                else
                {
                    post.RideNumber += rideNumber;
                }
                try
                {
                    await _postRepository.UpdateAsync(post);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.InnerException?.Message);
                }
            }
        }
    }
}
