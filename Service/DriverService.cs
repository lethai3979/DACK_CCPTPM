using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories;
using GoWheels_WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GoWheels_WebAPI.Service
{
    public class DriverService
    {
        private readonly DriverRepository _driverRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _userId;

        public DriverService(DriverRepository driverRepository
                                ,IHttpContextAccessor httpContextAccessor)
        {
            _driverRepository = driverRepository;
            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor.HttpContext?.User?
                        .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
        }

        public async Task<List<Driver>> GetAllAsync()
            => await _driverRepository.GetAllAsync();

        public async Task<Driver> GetByIdAsync(int id)
            => await _driverRepository.GetByIdAsync(id);

        public async Task<Driver> GetByUserIdAsync(string userId)
            => await _driverRepository.GetByUserIdAsync(userId);

        public async Task AddAsync(ApplicationUser user)
        {
            try
            {
                var driver = new Driver()
                {
                    CreatedOn = DateTime.Now,
                    CreatedById = _userId,
                    UserId = user.Id,
                    PricePerHour = 20000,
                    RatingPoint = 0
                };
                await _driverRepository.AddAsync(driver);
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

        public async Task UpdateAsync(int id, Driver driver)
        {
            try
            {
                driver.ModifiedById = _userId;
                driver.ModifiedOn = DateTime.Now;
                await _driverRepository.UpdateAsync(driver);
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

        public async Task DeleteByIdAsync(int id)
        {
            try
            {
                var driver = await _driverRepository.GetByIdAsync(id);
                driver.ModifiedById = _userId;
                driver.ModifiedOn = DateTime.Now;
                driver.IsDeleted = true;
                await _driverRepository.UpdateAsync(driver);
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