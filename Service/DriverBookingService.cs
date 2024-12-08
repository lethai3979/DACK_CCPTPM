using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories.Interface;
using GoWheels_WebAPI.Service.Interface;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GoWheels_WebAPI.Service
{
    public class DriverBookingService : IDriverBookingService
    {
        private readonly IDriverBookingRepository _driverBookingRepository;
        private readonly IDriverService _driverService;
        private readonly IInvoiceService _invoiceService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _userId;

        public DriverBookingService(IDriverBookingRepository driverBookingRepository,
                                    IDriverService driverService,
                                    IInvoiceService invoiceService,
                                    IHttpContextAccessor httpContextAccessor)
        {
            _driverBookingRepository = driverBookingRepository;
            _driverService = driverService;
            _invoiceService = invoiceService;
            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor.HttpContext?.User?
                        .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
        }

        public async Task<List<DriverBooking>> GetAllByUserIdAsync()
            => await _driverBookingRepository.GetAllByUserIdAsync(_userId);

        public async Task<DriverBooking> GetByIdAsync(int id)
            => await _driverBookingRepository.GetByIdAsync(id);

        public async Task<DriverBooking> GetByBookingIdAsync(int id)
            => await _driverBookingRepository.GetByBookingIdAsync(id);

        public async Task AddDriverBookingAsync(Booking booking)
        {
            try
            {
                var driver = await _driverService.GetByUserIdAsync(_userId);
                var driverBooking = new DriverBooking()
                {
                    CreatedById = _userId,
                    CreatedOn = DateTime.Now,
                    DriverId = driver.Id,
                    PickUpDate = booking.RecieveOn,
                    DropOffDate = booking.ReturnOn,
                    Total = driver.PricePerHour * (decimal)(booking.ReturnOn - booking.RecieveOn).TotalHours,
                };
                await _driverBookingRepository.AddAsync(driverBooking);
                await _invoiceService.AddDriverToInvocieAsync(booking, driverBooking);
            }
            catch (NullReferenceException nullEx)
            {
                throw new NullReferenceException(nullEx.Message);
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

        public async Task UpdateAsync(DriverBooking driverBooking)
        {
            try
            {
                driverBooking.ModifiedOn = DateTime.UtcNow;
                driverBooking.ModifiedById = _userId;
                await _driverBookingRepository.UpdateAsync(driverBooking);
            }
            catch (NullReferenceException nullEx)
            {
                throw new NullReferenceException(nullEx.Message);
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
