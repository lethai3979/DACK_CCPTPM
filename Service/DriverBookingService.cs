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

        public List<DriverBooking> GetAllByUserId()
            => _driverBookingRepository.GetAllByUserId(_userId);

        public DriverBooking GetById(int id)
            => _driverBookingRepository.GetById(id);

        public DriverBooking GetByBookingId(int id)
            => _driverBookingRepository.GetByBookingId(id);

        public void AddDriverBooking(Booking booking)
        {
            try
            {
                var driver = _driverService.GetByUserId(_userId);
                var driverBooking = new DriverBooking()
                {
                    CreatedById = _userId,
                    CreatedOn = DateTime.Now,
                    DriverId = driver.Id,
                    PickUpDate = booking.RecieveOn,
                    DropOffDate = booking.ReturnOn,
                    Total = driver.PricePerHour * (decimal)(booking.ReturnOn - booking.RecieveOn).TotalHours,
                };
                _driverBookingRepository.Add(driverBooking);
                _invoiceService.AddDriverToInvocie(booking, driverBooking);
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

        public void Update(DriverBooking driverBooking)
        {
            try
            {
                driverBooking.ModifiedOn = DateTime.UtcNow;
                driverBooking.ModifiedById = _userId;
                _driverBookingRepository.Update(driverBooking);
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
