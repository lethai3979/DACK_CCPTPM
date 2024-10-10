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
        private readonly string _userId;
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
            _userId = _httpContextAccessor.HttpContext?.User?
                        .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
        }

        public async Task<List<CarType>> GetAllAsync()
        {
            var carTypeList = await _carTypeRepository.GetAllAsync();
            if(carTypeList.Count == 0)
            {
                throw new NullReferenceException("List is empty");
            }
            return carTypeList;
        }

        public async Task<CarType> GetByIdAsync(int id)
            => await _carTypeRepository.GetByIdAsync(id);

        public async Task AddAsync(CarType carType, List<int> companyList)
        {
            try
            {
                if (companyList.Contains(0) || companyList.Count == 0)
                {
                    companyList.Clear();
                }
                carType.CreatedById = _userId;
                carType.CreatedOn = DateTime.Now;
                carType.IsDeleted = false;  
                await _carTypeRepository.AddAsync(carType);
                await _carTypeDetailRepository.AddCompaniesListAsync(carType.Id, companyList);
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
        
        public async Task DeleteByIdAsync(int id)
        {
            try
            {
                var carType = await _carTypeRepository.GetByIdAsync(id);
                carType.ModifiedById = _userId;
                carType.ModifiedOn = DateTime.Now;
                carType.IsDeleted = !carType.IsDeleted;
                await _carTypeRepository.UpdateAsync(carType);
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

        public async Task UpdateAsync(int id, CarType carType, List<int> companyIds)
        {
  
            try
            {
                var existingCarType = await _carTypeRepository.GetByIdAsync(id) ;
                carType.CreatedById = existingCarType.CreatedById;
                carType.CreatedOn = existingCarType.CreatedOn;
                carType.ModifiedById = existingCarType.ModifiedById;
                carType.ModifiedOn = existingCarType.ModifiedOn;


                if (companyIds.Contains(0) || companyIds.Count == 0)
                {
                    companyIds.Clear();
                }
                //Compare new CarTypeDetails with existing CarTypeDetails
                var isDetailsChange = await IsCarTypeDetailChange(companyIds, existingCarType.Id);
                if (isDetailsChange)
                {

                    await UpdateCarTypeDetails(existingCarType.Id, companyIds); 
                    EditHelper<CarType>.SetModifiedIfNecessary(carType, true, existingCarType, _userId);
                }
                else
                {
                    bool isCarTypeDataChange = EditHelper<CarType>
                                            .HasChanges(carType,existingCarType);//Check if CarType data changed
                    EditHelper<CarType>.SetModifiedIfNecessary(carType, isCarTypeDataChange, existingCarType, _userId); 
                }
                await _carTypeRepository.UpdateAsync(carType);
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
