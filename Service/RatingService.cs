using AutoMapper;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Repositories;
using GoWheels_WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace GoWheels_WebAPI.Service
{
    public class RatingService
    {
        private readonly RatingRepository _ratingRepository;
        private readonly PostService _postService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly string _userId;
        public RatingService(RatingRepository ratingAndCommentRepository, PostService postService, IHttpContextAccessor contextAccessor, IMapper mapper)
        {
            _ratingRepository = ratingAndCommentRepository;
            _postService = postService;
            _httpContextAccessor = contextAccessor;
            _mapper = mapper;
            _userId = _httpContextAccessor.HttpContext?.User?
                        .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
        }

/*        private async Task<float> GetAverageRatingFromPost(int postId)
        {
            var commentList = await _ratingRepository.GetAllRatingFromPost(postId);
            if (commentList == null || !commentList.Any())
            {
                return 0;
            }
            return commentList.Average(p => p.Point);
        }*/

        public async Task<OperationResult> AddAsync(RatingDTO ratingDto)
        {
            try
            {
                var rating = _mapper.Map<Rating>(ratingDto);
                rating.UserId = _userId;
                rating.CreatedById = _userId;
                rating.CreatedOn = DateTime.Now;
                await _ratingRepository.AddAsync(rating);
                var avgRating = await _ratingRepository.GetAveragePostRatingAsync(rating.PostId);
                await _postService.UpdatePostAverageRating(rating.PostId, avgRating);
                return new OperationResult(true, "Rating and comment added successfully", StatusCodes.Status201Created);
            }
            catch (DbUpdateException dbEx)
            {
                var dbExMessage = dbEx.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, dbExMessage,  StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                var exMessage = ex.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }

        public async Task<OperationResult> GetRatingsForPost(int postId)
        {
            try
            {
                var ratings = await _ratingRepository.GetAllRatingFromPost(postId);
                if (ratings == null || !ratings.Any())
                {
                    return new OperationResult(false, "No ratings found for this post", StatusCodes.Status404NotFound);
                }
                var ratingVMs = _mapper.Map<List<RatingVM>>(ratings);
                return new OperationResult(true, "Ratings retrieved successfully", StatusCodes.Status200OK, ratingVMs);
            }
            catch (Exception ex)
            {
                var exMessage = ex.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }
        public async Task<OperationResult> GetAllAsync()
        {
            var commentAndRatingList = await _ratingRepository.GetAllAsync();
            if (!commentAndRatingList.IsNullOrEmpty())
            {
                var commentAndRatingVM = _mapper.Map<List<RatingVM>>(commentAndRatingList);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: commentAndRatingVM);
            }
            return new OperationResult(message: "List empty", statusCode: StatusCodes.Status204NoContent);
        }

        public async Task<OperationResult> GetAllRatingFromPostId(int postId)
        {
            var commentAndRatingFromPost = await _ratingRepository.GetAllRatingFromPost(postId);
            if (!commentAndRatingFromPost.IsNullOrEmpty())
            {
                var commentAndRatingDTO = _mapper.Map<List<Rating>>(commentAndRatingFromPost);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: commentAndRatingDTO);
            }
            return new OperationResult(message: "List empty", statusCode: StatusCodes.Status204NoContent);
        }
        public async Task<OperationResult> DeleteByIdAsync(int id)
        {
            try
            {
                var comment = await _ratingRepository.GetByIdAsync(id);
                if (comment == null)
                {
                    return new OperationResult(false, "Comment not found", StatusCodes.Status404NotFound);
                }
                comment.IsDeleted = true;
                comment.ModifiedById = _userId;
                comment.ModifiedOn = DateTime.Now;
                await _ratingRepository.UpdateAsync(comment);
                return new OperationResult(true, "Comment deleted succesfully", StatusCodes.Status200OK);
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
            try
            {
                var rating = await _ratingRepository.GetByIdAsync(id);
                if (rating != null)
                {
                    var ratingAndCommentDTO = _mapper.Map<RatingVM>(rating);
                    return new OperationResult(true, "Comment found", StatusCodes.Status200OK, ratingAndCommentDTO);
                }
                return new OperationResult(false, "Comment not found", StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                var exMessage = ex.InnerException?.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }
        public async Task<OperationResult> UpdateAsync(int id, RatingDTO ratingDTO)
        {
            try
            {
                var existingRating = await _ratingRepository.GetByIdAsync(id);
                if (existingRating == null)
                {
                    return new OperationResult(true, "Rating not found", StatusCodes.Status404NotFound);
                }
                var rating = _mapper.Map<Rating>(ratingDTO);
                rating.CreatedOn = existingRating.CreatedOn;
                rating.CreatedById = existingRating.CreatedById;
                rating.ModifiedById = existingRating.ModifiedById;
                rating.ModifiedOn = existingRating.ModifiedOn;
                rating.UserId = existingRating.UserId;
                rating.User = existingRating.User;
                rating.PostId = existingRating.PostId;
                rating.Post = existingRating.Post;
                rating.IsDeleted = existingRating.IsDeleted;
                var isValueChange = EditHelper<Rating>.HasChanges(rating, existingRating);
                EditHelper<Rating>.SetModifiedIfNecessary(rating, isValueChange, existingRating, _userId);
                await _ratingRepository.UpdateAsync(rating);
                return new OperationResult(true, "Rating update succesfully", StatusCodes.Status200OK);
            }
            catch(DbUpdateException dbEx)
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
    }
}
    
