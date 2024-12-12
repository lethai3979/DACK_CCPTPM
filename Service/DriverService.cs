using GoWheels_WebAPI.Hubs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.GoogleRespone;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Repositories.Interface;
using GoWheels_WebAPI.Service.Interface;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text.Json;

namespace GoWheels_WebAPI.Service
{
    public class DriverService : IDriverService
    {
        private readonly IDriverRepository _driverRepository;
        private readonly INotifyService _notifyService;
        private readonly ILocatorService _googleApiService;
        private readonly IHubContext<NotifyHub> _hubcontext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _userId;

        public DriverService(IDriverRepository driverRepository,
                                INotifyService notifyService,
                                IHubContext<NotifyHub> hubContext,
                                IHttpContextAccessor httpContextAccessor,
                                ILocatorService googleApiService)
        {
            _driverRepository = driverRepository;
            _notifyService = notifyService;
            _hubcontext = hubContext;
            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor.HttpContext?.User?
                        .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
            _googleApiService = googleApiService;
        }

        public List<Driver> GetAll()
            => _driverRepository.GetAll();

        public Driver GetById(int id)
            => _driverRepository.GetById(id);

        public Driver GetByUserId(string userId)
            => _driverRepository.GetByUserId(userId);

        public void Add(ApplicationUser user)
        {
            try
            {
                var driver = new Driver()
                {
                    CreatedOn = DateTime.Now,
                    CreatedById = _userId,
                    UserId = user.Id,
                    PricePerHour = 70000,
                    RatingPoint = 0
                };
                _driverRepository.Add(driver);
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

        public void Update(int id, Driver driver)
        {
            try
            {
                driver.ModifiedById = _userId;
                driver.ModifiedOn = DateTime.Now;
                _driverRepository.Update(driver);
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

        public void DeleteById(int id)
        {
            try
            {
                var driver = _driverRepository.GetById(id);
                driver.ModifiedById = _userId;
                driver.ModifiedOn = DateTime.Now;
                driver.IsDeleted = true;
                _driverRepository.Update(driver);
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

        public async Task SendNotifyToDrivers(Booking booking)
        {
            try
            {
                var bookingLocationString = $"{booking.Latitude},{booking.Longitude}";
                var session = _httpContextAccessor.HttpContext!.Session;
                if(session == null)
                {
                    return;
                }    
                var userIds = session.Keys.ToList();
                var userLocations = new List<(string userId, string location)>();
                foreach (var userId in userIds)
                {
                    var userString = session.GetString(userId);
                    if (!userString.IsNullOrEmpty())
                    {
                        var user = JsonSerializer.Deserialize<UserVM>(userString!)!;
                        var userLocationString = $"{user.Latitude},{user.Longitude}";
                        userLocations.Add((userId, userLocationString));
                    }
                }
                var respone = await _googleApiService.GetDistanceAsync(userLocations, bookingLocationString);
                var usersInRange = GetUserWithinRange(respone, userLocations);

                foreach (var userId in usersInRange)
                {
                    var notify = new Notify()
                    {
                        CreateOn = DateTime.Now,
                        Content = "New booking nearby",
                        IsRead = false,
                        UserId = userId,
                        BookingId = booking.Id,
                    };
                    _notifyService.Add(notify);
                    if (NotifyHub.userConnectionsDic.TryGetValue(userId, out var connectionId))
                    {
                        await _hubcontext.Groups.AddToGroupAsync(connectionId, booking.Id.ToString());
                    }
                }
                await _hubcontext.Clients.Group(booking.Id.ToString()).SendAsync("ReceiveMessage", "System", "New booking nearby");
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

        private List<string> GetUserWithinRange(DistanceMatrixRespone distanceMatrixRespone, List<(string userId, string location)> userLocations)
        {
            var usersWithinRange = new List<string>();
            for(var i = 0; i < distanceMatrixRespone.Rows.Count; i++)
            {
                var distance = distanceMatrixRespone.Rows[i].Elements[0].Distance?.Value ?? int.MaxValue;
                if (distance < 10000)
                {
                    usersWithinRange.Add(userLocations[i].userId);
                }
            }    
            return usersWithinRange;
        }
    }
}