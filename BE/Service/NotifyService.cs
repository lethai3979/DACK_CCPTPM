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

        public List<Notify> GetAllByUserId()
            => _notifyRepository.GetAllByUserId(_userId);

        public Notify GetById(int id)
            => _notifyRepository.GetById(id);

        public void Add(Notify notify)
        {
            try
            {
                _notifyRepository.Add(notify);
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

        public void MarkAsRead(int id)
        {
            try
            {
                var notify = _notifyRepository.GetById(id);
                notify.IsRead = true;
                _notifyRepository.Update(notify);
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

        public void Delete(int id)
        {
            try
            {
                var notify = _notifyRepository.GetById(id);
                if (notify.UserId != _userId)
                {
                    throw new UnauthorizedAccessException("Unauthorize");
                }
                _notifyRepository.Delete(notify);
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
