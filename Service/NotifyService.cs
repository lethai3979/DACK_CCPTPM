using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories.Interface;
using GoWheels_WebAPI.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GoWheels_WebAPI.Service
{
    public class NotifyService : INotifyService
    {
        private readonly INotifyRepository _notifyRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _userId;

        public NotifyService(INotifyRepository notifyRepository, 
                                IHttpContextAccessor httpContextAccessor)
        {
            _notifyRepository = notifyRepository;
            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor.HttpContext?.User?
                        .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
        }

        public async Task<List<Notify>> GetAllByUserIdAsync()
            => await _notifyRepository.GetAllByUserIdAsync(_userId);
        
        public async Task<Notify> GetByIdAsync(int id)
            => await _notifyRepository.GetByIdAsync(id);

        public async Task AddAsync(Notify notify)
        {
            try
            {
                await _notifyRepository.AddAsync(notify);
            }
            catch (DbUpdateException dbEx)
            {
                throw new DbUpdateException(dbEx.Message);
            }
            catch (InvalidOperationException operationEx)
            {
                throw new InvalidOperationException(operationEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task MarkAsReadAsync(int id)
        {
            try
            {
                var notify = await _notifyRepository.GetByIdAsync(id);
                notify.IsRead = true;
                await _notifyRepository.UpdateAsync(notify);
            }
            catch (DbUpdateException dbEx)
            {
                throw new DbUpdateException(dbEx.Message);
            }
            catch (InvalidOperationException operationEx)
            {
                throw new InvalidOperationException(operationEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var notify = await _notifyRepository.GetByIdAsync(id);
                if(notify.UserId != _userId)
                {
                    throw new UnauthorizedAccessException("Unauthorize");
                }    
                await _notifyRepository.DeleteAsync(notify);
            }
            catch (DbUpdateException dbEx)
            {
                throw new DbUpdateException(dbEx.Message);
            }
            catch (InvalidOperationException operationEx)
            {
                throw new InvalidOperationException(operationEx.Message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
