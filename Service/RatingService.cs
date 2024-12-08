using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories.Interface;
using GoWheels_WebAPI.Service.Interface;
using GoWheels_WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GoWheels_WebAPI.Service
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IPostService _postService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _userId;
        public RatingService(IRatingRepository ratingAndCommentRepository, 
                                IPostService postService, 
                                IHttpContextAccessor contextAccessor)
        {
            _ratingRepository = ratingAndCommentRepository;
            _postService = postService;
            _httpContextAccessor = contextAccessor;
            _userId = _httpContextAccessor.HttpContext?.User?
                        .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
        }
        public async Task<List<Rating>> GetAllByPostId(int postId)
            => await _ratingRepository.GetAllByPostId(postId);


        public async Task<List<Rating>> GetAllAsync()
            => await _ratingRepository.GetAllAsync();

        public async Task<Rating> GetByIdAsync(int id)
            => await _ratingRepository.GetByIdAsync(id);

        public async Task AddAsync(Rating rating)
        {
            try
            {
                rating.UserId = _userId;
                rating.CreatedById = _userId;
                rating.CreatedOn = DateTime.Now;
                await _ratingRepository.AddAsync(rating);
                var avgRating = await _ratingRepository.GetAveragePostRatingAsync(rating.PostId);
                await _postService.UpdatePostAverageRatingAsync(rating.PostId, avgRating);
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
    
        public async Task DeleteByIdAsync(int id)
        {
            try
            {
                var comment = await _ratingRepository.GetByIdAsync(id);
                comment.IsDeleted = true;
                comment.ModifiedById = _userId;
                comment.ModifiedOn = DateTime.Now;
                await _ratingRepository.UpdateAsync(comment);
                var avgRating = await _ratingRepository.GetAveragePostRatingAsync(comment.PostId);
                await _postService.UpdatePostAverageRatingAsync(comment.PostId, avgRating);
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
        
        public async Task UpdateAsync(int id, Rating rating)
        {
            try
            {
                var existingRating = await _ratingRepository.GetByIdAsync(id);
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
                if (rating.Point != existingRating.Point) 
                {
                    var avgRating = await _ratingRepository.GetAveragePostRatingAsync(rating.PostId);
                    await _postService.UpdatePostAverageRatingAsync(rating.PostId, avgRating);
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
    }
}
    
