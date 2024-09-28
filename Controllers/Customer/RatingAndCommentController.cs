using AutoMapper;
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
    [Authorize(Roles = "User")]
    public class RatingAndCommentController : ControllerBase
    {
        private readonly RatingService _ratingAndCommentService;
        private readonly IMapper _mapper;
        public RatingAndCommentController(RatingService ratingAndCommentService, IMapper mapper)
        {
            _ratingAndCommentService = ratingAndCommentService;
            _mapper = mapper;
        }
        [HttpGet("GetAll")]
        public async Task<ActionResult<OperationResult>> GetAllAsync()
        {
            var result = await _ratingAndCommentService.GetAllAsync();
            return result;
        }

        [HttpGet("GetCommentFromPostId/{id}")]
        public async Task<ActionResult<OperationResult>> GetCommentFromPostId(int id)
        {
            var result = await _ratingAndCommentService.GetAllRatingFromPostId(id);
            return result;
        }

        [HttpGet("GetByIdAsync/{id}")]
        public async Task<ActionResult<OperationResult>> GetByIdAsync(int id)
        {
            var result = await _ratingAndCommentService.GetByIdAsync(id);
            return result;
        }
        [HttpPost("Add")]
        public async Task<ActionResult<OperationResult>> AddAsync([FromBody] RatingDTO ratingDTO)
        {
            if (ratingDTO == null)
            {
                return BadRequest("Comment cannot be empty");
            }
            if (ModelState.IsValid)
            {
                var result = await _ratingAndCommentService.AddAsync(ratingDTO);
                return result;
            }
            return BadRequest("Comment data invalid");
        }

        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult<OperationResult>> DeleteAsync(int id)
        {
            var result = await _ratingAndCommentService.DeleteByIdAsync(id);
            return result;
        }

        [HttpPost("Update/{id}")]
        public async Task<ActionResult<OperationResult>> UpdateAsync(int id, [FromBody] RatingDTO ratingDTO)
        {
            if (ratingDTO == null || id != ratingDTO.Id)
            {
                return BadRequest("Invalid request");
            }
            if (ModelState.IsValid)
            {
                var result = await _ratingAndCommentService.UpdateAsync(id, ratingDTO);
                return result;
            }
            return BadRequest("Comment data invalid");
        }
    }
}
