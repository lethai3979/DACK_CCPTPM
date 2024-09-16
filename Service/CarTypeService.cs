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
        private readonly CompanyRepository _companyRepository;
        private readonly CarTypeDetailRepository _carTypeDetailRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public CarTypeService(CarTypeRepository carTypeRepository,
                            CompanyRepository companyRepository,
                            CarTypeDetailRepository carTypeDetailRepository,
                            IMapper mapper, 
                            IHttpContextAccessor httpContextAccessor)
    {
            _carTypeRepository = carTypeRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _companyRepository = companyRepository;
            _carTypeDetailRepository = carTypeDetailRepository;
        }

        public async Task<OperationResult> GetAllAsync()
        {
            var carTypeList = await _carTypeRepository.GetAllAsync();
            if(carTypeList.Count != 0)
            {
                var carTypeListDTO = _mapper.Map<List<CarTypeDTO>>(carTypeList);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: carTypeListDTO);
            }

            return new OperationResult(message: "List empty", statusCode: StatusCodes.Status204NoContent);
        }

        public async Task<OperationResult> GetByIdAsync(int id)
        {
            var carType = await _carTypeRepository.GetByIdAsync(id);
            if (carType == null)
            {
                return new OperationResult(false, "Car type not found", StatusCodes.Status404NotFound);
            }
            return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: carType);
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
                await _carTypeDetailRepository.AddCompaniesListAsync(carType.Id, carTypeDTO.CompanyIds);
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

        public async Task<bool> IsCarTypeDetailChange(List<int> selectedCompanies, int existingCarTypeId)
        {
            var previousDetails = await _carTypeDetailRepository.GetCarTypeDetails(existingCarTypeId);
            var companies = await _companyRepository.GetAllAsync();
            foreach (var company in companies)
            {
                bool previosChecked = previousDetails != null && previousDetails.Any(c => c.Company.Id.Equals(company.Id));
                bool currentChecked = selectedCompanies.Contains(company.Id);
                if (previosChecked != currentChecked)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task<OperationResult> UpdateAsync(int id, CarTypeDTO carTypeDTO)
        {
            if(id != carTypeDTO.Id)
            {
                return new OperationResult(false, "Id invalid", StatusCodes.Status400BadRequest);
            }    
            try
            {
                var carType = _mapper.Map<CarType>(carTypeDTO);
                var existingCarType = await _carTypeRepository.GetByIdAsync(id);
                carType.CreateOn = existingCarType!.CreateOn;
                carType.CreateById = existingCarType.CreateById;
                carType.ModifiedById = existingCarType!.ModifiedById;
                carType.ModifiedOn = existingCarType.ModifiedOn;
                var isChange = await IsCarTypeDetailChange(carTypeDTO.CompanyIds, existingCarType.Id);
                if(isChange)
                {

                }
            }
            catch(DbUpdateException dbEx)
            {

            }
        }
    }
}
