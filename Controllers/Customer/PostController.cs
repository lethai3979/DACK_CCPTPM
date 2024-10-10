using AutoMapper;
using Azure;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Service;
using GoWheels_WebAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoWheels_WebAPI.Controllers.Customer
{
    [Area("User")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;
        private readonly IMapper _mapper;

        public PostController(PostService postService, IMapper mapper)
        {
            _postService = postService;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<OperationResult>> GetAllAsync()
        {
            try
            {
                var posts = await _postService.GetAllAsync();
                var postVMs = _mapper.Map<List<PostVM>>(posts)
            }

        }

        [HttpGet("GetPersonalPosts")]
        public async Task<ActionResult<OperationResult>> GetAllByUserId()
            => await _postService.GetByUserId();

        [HttpGet("GetByIdAsync/{id}")]
        public async Task<ActionResult<OperationResult>> GetByIdAsync(int id)
            => await _postService.GetByIdAsync(id);

        [HttpPost("Add")]
        public async Task<ActionResult<OperationResult>> AddAsync(PostDTO postDTO)
        {
            if (postDTO == null)
            {
                return BadRequest("Post data is null");
            }
            if (ModelState.IsValid)
            {
                var result = await _postService.AddAsync(postDTO);
                return result;
            }
            return BadRequest("Post data invalid");
        }

        [HttpPost("Update/{id}")]
        public async Task<ActionResult<OperationResult>> UpdateAsync(int id, PostDTO postDTO)
        {
            if (postDTO == null || id != postDTO.Id)
            {
                return BadRequest("Invalid request");
            }
            if (ModelState.IsValid)
            {
                var result = await _postService.UpdateAsync(id, postDTO);
                return result;
            }
            return BadRequest("Post data invalid");
        }

        [HttpPost("UpdateImages/{postId}")]
        public async Task<ActionResult<OperationResult>> UpdatePostImagesAsync(int postId, List<string> imagesUrls)
            => await _postService.UpdatePostImagesAsync(imagesUrls, postId);

        [HttpPost("Delete/{id}")]
        public async Task<ActionResult<OperationResult>> DeleteAsync(int id)
            => await _postService.DeleteByIdAsync(id);
    }
}
