using AutoMapper;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories;
using GoWheels_WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Service
{
    public class PostService
    {
        private readonly PostRepository _postRepository;
        private readonly PostAmenityRepository _postAmenityRepository;
        private readonly IMapper _mapper;

        public PostService(PostRepository postRepository, PostAmenityRepository postAmenityRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _postAmenityRepository = postAmenityRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult> AddAsync(PostDTO postDTO)
        {
            try
            {
                var post = _mapper.Map<Post>(postDTO);
                await _postRepository.AddAsync(post);
                await _postAmenityRepository.AddRangeAsync(postDTO.AmenitiesIdList, post.Id);
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

        public async Task<OperationResult> DeleteByIdAsync(int id)
        {
            try
            {
                var post = await _postRepository.GetByIdAsync(id);
                if(post == null)
                {
                    return new OperationResult(false, "Post not found", StatusCodes.Status404NotFound);
                }    
                await _postRepository.DeleteAsync(post);
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
            var postDTO = _mapper.Map<PostDTO>(post);
            return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: postDTO);
        }

        public async Task<OperationResult> GetAllAsync()
        {
            var postList = await _postRepository.GetAllAsync();
            if (postList.Count != 0)
            {
                var postListDTO = _mapper.Map<List<PostDTO>>(postList);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: postListDTO);
            }
            return new OperationResult(message: "List empty", statusCode: StatusCodes.Status204NoContent);
        }

    }
}
