using AutoMapper;
using GoWheels_WebAPI.Repositories;
using GoWheels_WebAPI.Utilities;
using GoWheels_WebAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Models.DTOs;

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
                var carTypeListVM = _mapper.Map<List<CarTypeVM>>(carTypeList);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: carTypeListVM);
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
            var carTypeVM = _mapper.Map<CarTypeVM>(carType);
            return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: carTypeVM);
        }

        public async Task<OperationResult> AddAsync(CarTypeDTO carTypeDTO)
        {
            try
            {
                if (carTypeDTO.CompanyIds.Contains(0))
                {
                    carTypeDTO.CompanyIds.Clear();
                }
                var carType = _mapper.Map<CarType>(carTypeDTO);
                carType.CreatedById = _httpContextAccessor.HttpContext?.User?
                        .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";//Get user Id
                carType.CreatedOn = DateTime.Now;
                carType.IsDeleted = false;  
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
                carType.ModifiedById = _httpContextAccessor.HttpContext?.User?
                        .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";//Get user Id
                carType.ModifiedOn = DateTime.Now;
                carType.IsDeleted = true;
                await _carTypeRepository.UpdateAsync(carType);
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

        private async Task<bool> IsCarTypeDetailChange(List<int> selectedCompanies, int existingCarTypeId)
        {
            var previousDetails = await _carTypeDetailRepository.GetCarTypeDetails(existingCarTypeId);
            var companies = await _companyRepository.GetAllAsync();
            foreach (var company in companies)
            {
                bool previousChecked = previousDetails != null && previousDetails.Any(c => c.CompanyId.Equals(company.Id));
                bool currentChecked = selectedCompanies.Contains(company.Id);
                if (previousChecked != currentChecked)
                {
                    return true;
                }
            }
            return false;
        }

        private async Task UpdateCarTypeDetails(int carTypeId, List<int> companyIds)
        {
            await _carTypeDetailRepository.ClearCarTypeDetailsAsync(carTypeId);//Clear previous CarTypeDetails
            await _carTypeDetailRepository.AddCompaniesListAsync(carTypeId, companyIds);//Add new CarTypeDetails
        }

        public async Task<OperationResult> UpdateAsync(int id, CarTypeDTO carTypeDTO)
        {
  
            try
            {
                //Check if cartype exist
                var existingCarType = await _carTypeRepository.GetByIdAsync(id);
                if (existingCarType == null) 
                {
                    return new OperationResult(false, "Car type not found", StatusCodes.Status404NotFound);
                }

                // Map DTO to CarType entity and retain the original creation metadata
                var carType = _mapper.Map<CarType>(carTypeDTO);
                carType.CreatedOn = existingCarType.CreatedOn;
                carType.CreatedById = existingCarType.CreatedById;
                carType.ModifiedById = existingCarType.ModifiedById;
                carType.ModifiedOn = existingCarType.ModifiedOn;

                //Compare new CarTypeDetails with existing CarTypeDetails
                if (carTypeDTO.CompanyIds.Contains(0))
                {
                    carTypeDTO.CompanyIds.Clear();
                }
                var isDetailsChange = await IsCarTypeDetailChange(carTypeDTO.CompanyIds, existingCarType.Id);
                if (isDetailsChange)
                {

                    await UpdateCarTypeDetails(existingCarType.Id, carTypeDTO.CompanyIds); 
                    EditHelper<CarType>.SetModifiedIfNecessary(carType, true, existingCarType, "NewUserId");
                }
                else
                {
                    bool isCarTypeDataChange = EditHelper<CarType>
                                            .HasChanges(carType,existingCarType);//Check if CarType data changed
                    EditHelper<CarType>.SetModifiedIfNecessary(carType, isCarTypeDataChange, existingCarType, "NewUserId"); 
                }
                await _carTypeRepository.UpdateAsync(carType);
                return new OperationResult(true, "Car type update succesfully", StatusCodes.Status200OK);
            }
            catch(DbUpdateException dbEx)
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
