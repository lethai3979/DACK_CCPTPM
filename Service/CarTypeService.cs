using AutoMapper;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Repositories;
using GoWheels_WebAPI.Utilities;
using GoWheels_WebAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace GoWheels_WebAPI.Service
{
    public class CarTypeService
    {
        private readonly CarTypeRepository _carTypeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public CarTypeService(CarTypeRepository carTypeRepository, 
                            IMapper mapper, 
                            IHttpContextAccessor httpContextAccessor)
    {
            _carTypeRepository = carTypeRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OperationResult> GetAllAsync()
        {
            var carTypeList = await _carTypeRepository.GetAllAsync();
            if(carTypeList.Count != 0)
            {
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: carTypeList);
            }
            return new OperationResult(message: "List empty", statusCode: StatusCodes.Status204NoContent);
        }


        public async Task<OperationResult> AddAsync(CarTypeDTO carTypeDTO)
        {
            carTypeDTO.CreateById = _httpContextAccessor.HttpContext?.User?
                                    .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";//Get user Id
            carTypeDTO.CreateOn = DateTime.Now;
            try
            {
                var carType = _mapper.Map<CarType>(carTypeDTO);
                await _carTypeRepository.AddAsync(carType);
                await _carTypeRepository.AddCarTypeDetail(carType.Id, carTypeDTO.CompanyIds);
                return new OperationResult(true, "Car type add succesfully", StatusCodes.Status200OK);
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
        
        public async Task<OperationResult> DeletedByIdAsync(int id)
        {
            try
            {
                var carType = await _carTypeRepository.GetByIdAsync(id);
                if(carType == null)
                {
                    return new OperationResult(false, "Car type not found", StatusCodes.Status404NotFound);
                }    
                await _carTypeRepository.DeleteAsync(carType);
                return new OperationResult(true, "Car type deleted succesfully", StatusCodes.Status200OK);
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
    }
}
