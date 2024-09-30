using AutoMapper;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Repositories;
using GoWheels_WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GoWheels_WebAPI.Service
{
    public class FavoriteService
    {
        private readonly FavoriteRepository _favoriteRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly string _userId;

        public FavoriteService(FavoriteRepository favoriteRepository,
                                IHttpContextAccessor httpContextAccessor, 
                                IMapper mapper)
        {
            _favoriteRepository = favoriteRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _userId = _httpContextAccessor.HttpContext?.User?
                     .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
        }

        public async Task<OperationResult> AddAsync(FavoriteDTO favoriteDTO)
        {
            try
            {
                var favorite = await _favoriteRepository.GetByPostIdAsync(favoriteDTO.PostId, _userId);
                if (favorite == null) {
                    favorite = _mapper.Map<Favorite>(favoriteDTO);
                    favorite.UserId = _userId;
                    favorite.IsDeleted = false;
                    await _favoriteRepository.AddToFavoriteAsync(favorite);
                    return new OperationResult(true, "Favorite add succesfully", StatusCodes.Status200OK);
                }
                favorite.IsDeleted = false;
                await _favoriteRepository.UpdateFavoriteAsync(favorite);
                return new OperationResult(true, "Favorite add succesfully", StatusCodes.Status200OK);
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

        public async Task<OperationResult> DeletedAsync(int id)
        {
            try
            {
                var favorite = await _favoriteRepository.GetByIdAsync(id, _userId);
                if(favorite == null)
                {
                    return new OperationResult(false, "Favorite not found", StatusCodes.Status404NotFound);
                }
                favorite.IsDeleted = true;
                await _favoriteRepository.UpdateFavoriteAsync(favorite);
                return new OperationResult(true, "Favorite remove succesfully", StatusCodes.Status200OK);
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

        public async Task<OperationResult> GetAllAsync()
        {
                var favorites = await _favoriteRepository.GetAllFavoriteAsync(_userId);
                if(favorites.Count !=0)
                {
                    var favoriteVMs = _mapper.Map<List<FavoriteVM>>(favorites);
                    return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: favoriteVMs);
                }
                return new OperationResult(message: "List empty", statusCode: StatusCodes.Status204NoContent);
        }
    }
}
