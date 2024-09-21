using AutoMapper;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories;
using GoWheels_WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace GoWheels_WebAPI.Service
{
    public class RatingAndCommentService
    {
        private readonly RatingAndCommentRepository _ratingAndCommentRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;
        public RatingAndCommentService(RatingAndCommentRepository ratingAndCommentRepository, IHttpContextAccessor contextAccessor, IMapper mapper)
        {
            _ratingAndCommentRepository = ratingAndCommentRepository;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
        }

        private async Task<float> GetAverageRatingFromPost(int postId)
        {
            var commentList = await _ratingAndCommentRepository.GetAllCommentFromPost(postId);
            if (commentList == null || !commentList.Any())
            {
                return 0;
            }
            return commentList.Average(p => p.Point);
        }

        public async Task<OperationResult> AddRatingAndComment(RatingDTO ratingDto , int PostId)
        {
            try
            {
                var rating = _mapper.Map<Rating>(ratingDto);
                rating.UserId = _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
                rating.PostId = PostId;
                rating.CreatedOn = DateTime.Now;
                await _ratingAndCommentRepository.AddAsync(rating);
                return new OperationResult(true, "Rating and comment added successfully", StatusCodes.Status201Created);
            }
            catch (DbUpdateException dbEx)
            {
                return new OperationResult(false, $"Database error: {dbEx.Message}", StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, $"An error occurred: {ex.Message}", StatusCodes.Status400BadRequest);
            }
        }

        public async Task<OperationResult> GetRatingsForPost(int postId)
        {
            try
            {
                var ratings = await _ratingAndCommentRepository.GetAllCommentFromPost(postId);
                if (ratings == null || !ratings.Any())
                {
                    return new OperationResult(false, "No ratings found for this post", StatusCodes.Status404NotFound);
                }
                var ratingDtos = _mapper.Map<List<RatingDTO>>(ratings);
                return new OperationResult(true, "Ratings retrieved successfully", StatusCodes.Status200OK, ratingDtos);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, $"An error occurred: {ex.Message}", StatusCodes.Status500InternalServerError);
            }
        }
        public async Task<OperationResult> GetAllAsync()
        {
            var commentAndRatingList = await _ratingAndCommentRepository.GetAllAsync();
            if (!commentAndRatingList.IsNullOrEmpty())
            {
                var commentAndRatingDTO = _mapper.Map<List<Rating>>(commentAndRatingList);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: commentAndRatingDTO);
            }
            return new OperationResult(message: "List empty", statusCode: StatusCodes.Status204NoContent);
        }

        public async Task<OperationResult> GetAllCommentAndRatingFormPostId(int postId)
        {
            var commentAndRatingFromPost = await _ratingAndCommentRepository.GetAllCommentFromPost(postId);
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
                var comment = await _ratingAndCommentRepository.GetByIdAsync(id);
                if (comment == null)
                {
                    return new OperationResult(false, "Comment not found", StatusCodes.Status404NotFound);
                }
                await _ratingAndCommentRepository.DeleteAsync(comment);
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
                var rating = await _ratingAndCommentRepository.GetByIdAsync(id);
                if (rating != null)
                {
                    var ratingAndCommentDTO = _mapper.Map<RatingDTO>(rating);
                    return new OperationResult(true, "Comment found", StatusCodes.Status200OK, ratingAndCommentDTO);
                }
                return new OperationResult(false, "Comment not found", StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, $"An error occurred: {ex.Message}", StatusCodes.Status500InternalServerError);
            }
        }
        //public async Task<OperationResult> UpdateAsync(int id, RatingDTO rating)
        //{
        //    var exsistingComment = _ratingAndCommentRepository.GetByIdAsync(id);
        //    if(exsistingComment != null) 
        //    {

        //    }
        //}
    }
}
    
