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

        public async Task<List<Favorite>> GetAllAsync()
        {
            var favorites = await _favoriteRepository.GetAllByUserIdAsync(_userId);
            if (favorites.Count != 0)
            {
                throw new NullReferenceException("List is empty");
            }
            return favorites;
        }

        public async Task AddAsync(Favorite favorite)
        {
            try
            {
                var existingFavorite = await _favoriteRepository.GetByPostIdAsync(favorite.PostId, _userId);
                if (existingFavorite == null) {
                    existingFavorite = _mapper.Map<Favorite>(favorite);
                    existingFavorite.UserId = _userId;
                    existingFavorite.IsDeleted = false;
                    await _favoriteRepository.AddAsync(existingFavorite);
                }
                else
                {
                    existingFavorite.IsDeleted = false;
                    await _favoriteRepository.UpdateAsync(existingFavorite);
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

        public async Task DeletedAsync(int id)
        {
            try
            {
                var favorite = await _favoriteRepository.GetByIdAsync(id, _userId);
                favorite.IsDeleted = true;
                await _favoriteRepository.UpdateAsync(favorite);
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
