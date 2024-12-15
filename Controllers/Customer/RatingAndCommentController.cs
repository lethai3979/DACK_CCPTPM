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
        public ActionResult<OperationResult> GetAll()
        {
            try
            {
                var ratings = _ratingAndCommentService.GetAll();
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
        public ActionResult<OperationResult> GetCommentByPostId(int id)
        {
            try
            {
                var ratings = _ratingAndCommentService.GetAllByPostId(id);
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

        [HttpGet("GetById/{id}")]
        public ActionResult<OperationResult> GetById(int id)
        {
            try
            {
                var rating = _ratingAndCommentService.GetById(id);
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
        public ActionResult<OperationResult> Add([FromBody] RatingDTO ratingDTO)
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
                    _ratingAndCommentService.Add(rating);
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
        public ActionResult<OperationResult> Delete(int id)
        {
            try
            {
                _ratingAndCommentService.DeleteById(id);
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

        [HttpPost("UpdateTrustLevel/{id}")]
        [Authorize]
        public ActionResult<OperationResult> Update(int id, [FromBody] RatingDTO ratingDTO)
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
                    _ratingAndCommentService.Update(id, rating);
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
