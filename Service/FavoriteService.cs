using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories.Interface;
using GoWheels_WebAPI.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GoWheels_WebAPI.Service
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _userId;

        public FavoriteService(IFavoriteRepository favoriteRepository,
                                IHttpContextAccessor httpContextAccessor)
        {
            _favoriteRepository = favoriteRepository;
            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor.HttpContext?.User?
                     .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
        }

        public async Task<List<Favorite>> GetAllAsync()
            => await _favoriteRepository.GetAllByUserIdAsync(_userId);


        public async Task AddAsync(Favorite favorite)
        {
            try
            {
                var existingFavorite = await _favoriteRepository.GetByPostIdAsync(favorite.PostId, _userId);
                if (existingFavorite == null)
                {
                    favorite.UserId = _userId;
                    favorite.IsDeleted = false;
                    await _favoriteRepository.AddAsync(favorite);
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
