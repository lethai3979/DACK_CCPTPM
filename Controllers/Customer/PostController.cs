using AutoMapper;
using Azure;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Service;
using GoWheels_WebAPI.Service.Interface;
using GoWheels_WebAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Controllers.Customer
{
    [Area("User")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IMapper _mapper;

        public PostController(IPostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public ActionResult<OperationResult> GetAll(int? pageIndex, [FromQuery] SearchFilterModel filterModel)
        {
            try
            {
                int currentPageIndex = pageIndex ?? 1;
                var posts = _postService.GetAll();
                posts = _postService.ApplyFilters(posts, filterModel);
                var paginateList = PaginatedList<Post>.Create(posts, currentPageIndex, 8);
                var postVMs = _mapper.Map<List<PostVM>>(paginateList);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: postVMs);
            }
            catch (NullReferenceException nullEx)
            {
                return new OperationResult(false, nullEx.Message, StatusCodes.Status204NoContent);
            }
            catch (AutoMapperMappingException mapperEx)
            {
                return new OperationResult(false, mapperEx.Message, StatusCodes.Status422UnprocessableEntity);
            }
            catch (Exception ex)
            {
                var exMessage = ex.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }

        [HttpGet("GetTotalPost")]
        public IActionResult GetTotalPost()
            => Ok(_postService.GetTotalPost());

        [HttpGet("GetPersonalPosts")]
        [Authorize(Roles = "User")]
        public ActionResult<OperationResult> GetAllByUserId()
        {
            try
            {
                var posts = _postService.GetAllByUserId();
                var postVMs = _mapper.Map<List<PostVM>>(posts);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: postVMs);
            }
            catch (NullReferenceException nullEx)
            {
                return new OperationResult(false, nullEx.Message, StatusCodes.Status204NoContent);
            }
            catch (AutoMapperMappingException mapperEx)
            {
                return new OperationResult(false, mapperEx.Message, StatusCodes.Status422UnprocessableEntity);
            }
            catch (Exception ex)
            {
                var exMessage = ex.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }

        [HttpGet("GetAllByUserId/{userId}")]
        public ActionResult<OperationResult> GetAllByUserId(string userId)
        {
            try
            {
                var posts = _postService.GetAllByUserId(userId);
                var postVMs = _mapper.Map<List<PostVM>>(posts);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: postVMs);
            }
            catch (NullReferenceException nullEx)
            {
                return new OperationResult(false, nullEx.Message, StatusCodes.Status204NoContent);
            }
            catch (AutoMapperMappingException mapperEx)
            {
                return new OperationResult(false, mapperEx.Message, StatusCodes.Status422UnprocessableEntity);
            }
            catch (Exception ex)
            {
                var exMessage = ex.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }

        [HttpGet("GetById/{id}")]
        public ActionResult<OperationResult> GetById(int id)
        {
            try
            {
                var post = _postService.GetById(id);
                var postVM = _mapper.Map<PostVM>(post);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: postVM);
            }
            catch (NullReferenceException nullEx)
            {
                return new OperationResult(false, nullEx.Message, StatusCodes.Status204NoContent);
            }
            catch (AutoMapperMappingException mapperEx)
            {
                return new OperationResult(false, mapperEx.Message, StatusCodes.Status422UnprocessableEntity);
            }
            catch (Exception ex)
            {
                var exMessage = ex.Message ?? "An error occurred while updating the database.";
                return new OperationResult(false, exMessage, StatusCodes.Status400BadRequest);
            }
        }

        [HttpPost("Add")]
        [Authorize(Roles = "User")]
        public ActionResult<OperationResult> Add([FromForm] PostDTO postDTO)
        {
            try
            {
                if (postDTO == null)
                {
                    return BadRequest("Post data is null");
                }
                if (ModelState.IsValid)
                {
                    var post = _mapper.Map<Post>(postDTO);
                    _postService.Add(post, postDTO.Image!, postDTO.ImagesList!, postDTO.AmenitiesIds);
                    return new OperationResult(true, "Post add succesfully", StatusCodes.Status200OK);
                }
                return BadRequest("Post data invalid");
            }
            catch (DbUpdateException dbEx)
            {
                return new OperationResult(false, dbEx.Message, StatusCodes.Status500InternalServerError);
            }
            catch (InvalidOperationException operationEx)
            {
                return new OperationResult(false, operationEx.Message, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, StatusCodes.Status400BadRequest);
            }
        }

        [HttpPut("Update/{id}")]
        [Authorize(Roles = "User")]
        public ActionResult<OperationResult> Update(int id, [FromForm] PostDTO postDTO)
        {
            try
            {
                if (postDTO == null || id != postDTO.Id)
                {
                    return BadRequest("Invalid request");
                }
                if (ModelState.IsValid)
                {
                    var post = _mapper.Map<Post>(postDTO);
                    _postService.Update(id, post, postDTO.Image!, postDTO.AmenitiesIds);
                    _postService.UpdatePostImages(postDTO.ImagesList!, id);
                    return new OperationResult(true, "Post update succesfully", StatusCodes.Status200OK);
                }
                return BadRequest("Post data invalid");
            }
            catch (NullReferenceException nullEx)
            {
                return new OperationResult(false, nullEx.Message, StatusCodes.Status204NoContent);
            }
            catch (DbUpdateException dbEx)
            {
                return new OperationResult(false, dbEx.Message, StatusCodes.Status500InternalServerError);
            }
            catch (UnauthorizedAccessException authEx)
            {
                return new OperationResult(false, authEx.Message, StatusCodes.Status401Unauthorized);
            }
            catch (InvalidOperationException operationEx)
            {
                return new OperationResult(false, operationEx.Message, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, StatusCodes.Status400BadRequest);
            }
        }

        [HttpPost("Delete/{id}")]
        [Authorize(Roles = "User")]
        public ActionResult<OperationResult> Delete(int id)
        {
            try
            {
                _postService.DeleteById(id);
                return new OperationResult(true, "Post deleted succesfully", StatusCodes.Status200OK);
            }
            catch (NullReferenceException nullEx)
            {
                return new OperationResult(false, nullEx.Message, StatusCodes.Status204NoContent);
            }
            catch (DbUpdateException dbEx)
            {
                return new OperationResult(false, dbEx.Message, StatusCodes.Status500InternalServerError);
            }
            catch (UnauthorizedAccessException authEx)
            {
                return new OperationResult(false, authEx.Message, StatusCodes.Status401Unauthorized);
            }
            catch (InvalidOperationException operationEx)
            {
                return new OperationResult(false, operationEx.Message, StatusCodes.Status500InternalServerError);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, ex.Message, StatusCodes.Status400BadRequest);
            }
        }
    }
}
