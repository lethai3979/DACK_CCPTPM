using Azure;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Service;
using GoWheels_WebAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GoWheels_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = ApplicationRole.User)]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;

        public PostController(PostService postService)
        {
            _postService = postService;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<OperationResult>> GetAllAsync()
            => await _postService.GetAllAsync();

        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<OperationResult>> GetByIdAsync(int id)
            => await _postService.GetByIdAsync(id);

        [HttpPost("Add")]
        public async Task<ActionResult<OperationResult>> AddAsync(PostDTO postDTO)
        {
            if(postDTO == null) 
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

        [HttpPost("Delete/{id}")]
        public async Task<ActionResult<OperationResult>> DeleteAsync(int id)
            => await _postService.DeleteByIdAsync(id);
    }
}
