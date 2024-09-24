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
        private readonly AmenityRepository _amenityRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private  readonly string _userId;

        public PostService(PostRepository postRepository,
                            PostAmenityRepository postAmenityRepository,
                            AmenityRepository amenityRepository,
                            IHttpContextAccessor httpContextAccessor,
                            IMapper mapper)
        {
            _postRepository = postRepository;
            _postAmenityRepository = postAmenityRepository;
            _amenityRepository = amenityRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _userId = _httpContextAccessor.HttpContext?.User?
                        .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
        }

        public async Task<OperationResult> AddAsync(PostDTO postDTO)
        {

            try
            {
                var post = _mapper.Map<Post>(postDTO);
                post.CreatedById = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                post.CreatedOn = DateTime.Now;
                post.UserId = _userId;
                post.IsDeleted = false;
                post.IsAvailable = true;
                await _postRepository.AddAsync(post);
                await _postRepository.AddPostImagesAsync(postDTO.ImageUrls, post.Id);
                await _postAmenityRepository.AddRangeAsync(postDTO.PostAmenitiesIds, post.Id);
                return new OperationResult(true, "Post add succesfully", StatusCodes.Status200OK);
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


        private async Task<bool> IsPostAmenityChange(List<int> selectedAmenities, int existingPostId)
        {
            var previousDetails = await _postAmenityRepository.GetAmenityByPostIdAsync(existingPostId);
            var amenities = await _amenityRepository.GetAllAsync();
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

        private async Task UpdatePostAmenitiesAsync(int postId, List<int> amenitiesIDs)
        {
            await _postAmenityRepository.RemoveRangeAsync(postId);
            await _postAmenityRepository.AddRangeAsync(amenitiesIDs, postId);
        }

        public async Task<OperationResult> UpdateAsync(int id, PostDTO postDTO)
        {
            try
            {
                var existingPost = await _postRepository.GetByIdAsync(id);
                if(existingPost == null)
                {
                    return new OperationResult(false, "Post not found", StatusCodes.Status404NotFound);
                }
                var post = _mapper.Map<Post>(postDTO);
                post.CreatedOn = existingPost.CreatedOn;
                post.CreatedById = existingPost.CreatedById;
                post.ModifiedById = existingPost.ModifiedById;
                post.ModifiedOn = existingPost.ModifiedOn;

                if(postDTO.PostAmenitiesIds.Contains(0))
                {
                    postDTO.PostAmenitiesIds.Clear();
                }
                var isPostAmenitiesChange = await IsPostAmenityChange(postDTO.PostAmenitiesIds, existingPost.Id);
                if(isPostAmenitiesChange)
                {
                    await UpdatePostAmenitiesAsync(existingPost.Id, postDTO.PostAmenitiesIds);
                    EditHelper<Post>.SetModifiedIfNecessary(post, true, existingPost, _userId);
                }    
                else
                {
                    var isPostDataChange = EditHelper<Post>.HasChanges(post, existingPost);
                    EditHelper<Post>.SetModifiedIfNecessary(post, isPostDataChange, existingPost, _userId);
                }
                await _postRepository.UpdateAsync(post);
                return new OperationResult(true, "Post update succesfully", StatusCodes.Status200OK);
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

        public async Task<OperationResult> UpdatePostImagesAsync(List<string> imageUrl, int postId)
        {
            try
            {
                var post = await _postRepository.GetByIdAsync(postId);
                if(post == null)
                {
                    return new OperationResult(false, "Post not found", StatusCodes.Status404NotFound);
                }
                post.ModifiedById = _httpContextAccessor.HttpContext?.User?
                        .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
                post.ModifiedOn = DateTime.Now;
                await _postRepository.UpdateAsync(post);
                await _postRepository.DeletePosImagesAsync(post.Id);
                await _postRepository.AddPostImagesAsync(imageUrl, postId);
                return new OperationResult(true, "Post images update succesfully", StatusCodes.Status200OK);
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

        public async Task<OperationResult> DeleteByIdAsync(int id)
        {
            try
            {
                var post = await _postRepository.GetByIdAsync(id);
                if(post == null)
                {
                    return new OperationResult(false, "Post not found", StatusCodes.Status404NotFound);
                }    
                post.IsDeleted = true;
                post.ModifiedById = _httpContextAccessor.HttpContext?.User?
                    .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
                post.ModifiedOn = DateTime.Now; 
                await _postRepository.UpdateAsync(post);
                return new OperationResult(true, "Post delete succesfully", StatusCodes.Status200OK);
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


        public async Task<OperationResult> GetByIdAsync(int id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            if (post == null)
            {
                return new OperationResult(false, "Post not found", StatusCodes.Status404NotFound);
            }
            var postVM = _mapper.Map<PostVM>(post);
            return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: postVM);
        }

        public async Task<OperationResult> GetAllAsync()
        {
            var postList = await _postRepository.GetAllAsync();
            if (postList.Count != 0)
            {
                var postListVM = _mapper.Map<List<PostVM>>(postList);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: postListVM);
            }
            return new OperationResult(message: "List empty", statusCode: StatusCodes.Status204NoContent);
        }

    }
}
