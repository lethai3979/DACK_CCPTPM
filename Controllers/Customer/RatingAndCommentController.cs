using AutoMapper;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Service;
using GoWheels_WebAPI.Service.Interface;
using GoWheels_WebAPI.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GoWheels_WebAPI.Controllers.Customer
{
    [Area("User")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class RatingAndCommentController : ControllerBase
    {
        private readonly IRatingService _ratingAndCommentService;
        private readonly IMapper _mapper;
        public RatingAndCommentController(IRatingService ratingAndCommentService, IMapper mapper)
        {
            _ratingAndCommentService = ratingAndCommentService;
            _mapper = mapper;
        }
        [HttpGet("GetAll")]
        public async Task<ActionResult<OperationResult>> GetAllAsync()
        {
            try
            {
                var ratings = await _ratingAndCommentService.GetAllAsync();
                var ratingVMs = _mapper.Map<List<RatingVM>>(ratings);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: ratingVMs);
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

        [HttpGet("GetCommentByPostId/{id}")]
        public async Task<ActionResult<OperationResult>> GetCommentByPostId(int id)
        {
            try
            {
                var ratings = await _ratingAndCommentService.GetAllByPostId(id);
                var ratingVMs = _mapper.Map<List<RatingVM>>(ratings);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: ratingVMs);
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

        [HttpGet("GetByIdAsync/{id}")]
        public async Task<ActionResult<OperationResult>> GetByIdAsync(int id)
        {
            try
            {
                var rating = await _ratingAndCommentService.GetByIdAsync(id);
                var ratingVM = _mapper.Map<RatingVM>(rating);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: ratingVM);
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
        [Authorize]
        public async Task<ActionResult<OperationResult>> AddAsync([FromBody] RatingDTO ratingDTO)
        {
            try
            {
                if (ratingDTO == null)
                {
                    return BadRequest("Comment cannot be empty");
                }
                if (ModelState.IsValid)
                {
                    var rating = _mapper.Map<Rating>(ratingDTO);
                    await _ratingAndCommentService.AddAsync(rating);
                    return new OperationResult(true, "Comment add succesfully", StatusCodes.Status200OK);
                }
                return BadRequest("Comment data invalid");
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

        [HttpDelete("Delete/{id}")]
        [Authorize]
        public async Task<ActionResult<OperationResult>> DeleteAsync(int id)
        {
            try
            {
                await _ratingAndCommentService.DeleteByIdAsync(id);
                return new OperationResult(true, "Comment deleted succesfully", StatusCodes.Status200OK);

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

        [HttpPost("Update/{id}")]
        [Authorize]
        public async Task<ActionResult<OperationResult>> UpdateAsync(int id, [FromBody] RatingDTO ratingDTO)
        {
            try
            {
                if (ratingDTO == null || id != ratingDTO.Id)
                {
                    return BadRequest("Invalid request");
                }
                if (ModelState.IsValid)
                {
                    var rating = _mapper.Map<Rating>(ratingDTO);
                    await _ratingAndCommentService.UpdateAsync(id, rating);
                    return new OperationResult(true, "Comment update succesfully", StatusCodes.Status200OK);
                }
                return BadRequest("Comment data invalid");
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
    }
}
