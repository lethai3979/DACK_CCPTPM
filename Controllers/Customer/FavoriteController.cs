using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
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
    public class FavoriteController : ControllerBase
    {
        private readonly FavoriteService _favoriteService;

        public FavoriteController(FavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpPost("AddToFavorite")]
        public async Task<ActionResult<OperationResult>> AddAsync(FavoriteDTO favoriteDTO)
        {
            if (favoriteDTO == null)
            {
                return BadRequest("Invalid request");
            }
            if (ModelState.IsValid)
            {
                var result = await _favoriteService.AddAsync(favoriteDTO);
                return result;
            }
            return BadRequest("Favorite data invalid");
        }

        [HttpDelete("RemoveFavorite/{id}")]
        public async Task<ActionResult<OperationResult>> RemoveAsync(int id)
            => await _favoriteService.DeletedAsync(id);

        [HttpGet("GetAllFavorite")]
        public async Task<ActionResult<OperationResult>> GetAllAsync()
            => await _favoriteService.GetAllAsync();
    }
}
