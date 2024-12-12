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
        public List<Rating> GetAllByPostId(int postId)
            => _ratingRepository.GetAllByPostId(postId);


        public List<Rating> GetAll()
            => _ratingRepository.GetAll();

        public Rating GetById(int id)
            => _ratingRepository.GetById(id);

        public void Add(Rating rating)
        {
            try
            {
                rating.UserId = _userId;
                rating.CreatedById = _userId;
                rating.CreatedOn = DateTime.Now;
                _ratingRepository.Add(rating);
                var avgRating = _ratingRepository.GetAveragePostRating(rating.PostId);
                _postService.UpdatePostAverageRating(rating.PostId, avgRating);
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

        public void DeleteById(int id)
        {
            try
            {
                var comment = _ratingRepository.GetById(id);
                comment.IsDeleted = true;
                comment.ModifiedById = _userId;
                comment.ModifiedOn = DateTime.Now;
                _ratingRepository.Update(comment);
                var avgRating = _ratingRepository.GetAveragePostRating(comment.PostId);
                _postService.UpdatePostAverageRating(comment.PostId, avgRating);
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

        public void Update(int id, Rating rating)
        {
            try
            {
                var existingRating = _ratingRepository.GetById(id);
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
                _ratingRepository.Update(rating);
                if (rating.Point != existingRating.Point)
                {
                    var avgRating = _ratingRepository.GetAveragePostRating(rating.PostId);
                    _postService.UpdatePostAverageRating(rating.PostId, avgRating);
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
    
