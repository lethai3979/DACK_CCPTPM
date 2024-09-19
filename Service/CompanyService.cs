using AutoMapper;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories;
using GoWheels_WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace GoWheels_WebAPI.Service
{
    public class CompanyService
    {
        private readonly CompanyRepository _companyRepository;
        private readonly CarTypeDetailRepository _carTypeDetailRepository;
        private readonly CarTypeRepository _carTypeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public CompanyService(CompanyRepository companyRepository, 
                            IMapper mapper, 
                            IHttpContextAccessor httpContextAccessor, 
                            CarTypeDetailRepository carTypeDetailRepository,
                            CarTypeRepository carTypeRepository)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _carTypeDetailRepository = carTypeDetailRepository;
            _carTypeRepository = carTypeRepository;
        }

        public async Task<OperationResult> GetByIdAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null)
            {
                return new OperationResult(false, "Company not found", StatusCodes.Status404NotFound);
            }
            var companyDTO = _mapper.Map<CompanyDTO>(company);
            return new OperationResult(true,statusCode: StatusCodes.Status200OK, data: company);
        }

        public async Task<OperationResult> GetAllAsync()
        {
            var companies = await _companyRepository.GetAllAsync();
            if(companies.Count == 0)
            {
               return new OperationResult(false, "List is empty", StatusCodes.Status404NotFound);
            }
            var companiesDTO = _mapper.Map<List<CompanyDTO>>(companies);
            return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: companiesDTO);
        }

        public async Task<OperationResult> AddAsync(CompanyDTO companyDTO)
        {
            companyDTO.CreatedById = _httpContextAccessor.HttpContext?.User?
                                    .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";//Get user Id
            companyDTO.CreatedOn = DateTime.Now;
            try
            {
                var company = _mapper.Map<Company>(companyDTO);
                await _companyRepository.AddAsync(company);
                await _companyRepository.AddCompanyDetailAsync(company.Id, companyDTO.CarTypeIds);
                return new OperationResult(true, "Company add successfully",StatusCodes.Status200OK);
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

        public async Task<OperationResult> DeleteByIdAsync(int id)
        {
            try
            {
                var carType = await _companyRepository.GetByIdAsync(id);
                if (carType == null)
                {
                    return new OperationResult(false, "Company not found", StatusCodes.Status404NotFound);
                }
                await _companyRepository.DeleteAsync(carType);
                return new OperationResult(true, "Company deleted succesfully", StatusCodes.Status200OK);
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

        public async Task<OperationResult> UpdateAsync(int id, CompanyDTO companyDTO)
        {
            try
            {
                //Check if Company exist
                var existingCompany = await _companyRepository.GetByIdAsync(id);
                if (existingCompany == null)
                {
                    return new OperationResult(false, "Car type not found", StatusCodes.Status404NotFound);
                }

                // Map DTO to Company entity and retain the original creation metadata
                var company = _mapper.Map<Company>(companyDTO);
                company.CreatedOn = existingCompany.CreatedOn;
                company.CreatedById = existingCompany.CreatedById;
                company.ModifiedById = existingCompany.ModifiedById;
                company.ModifiedOn = existingCompany.ModifiedOn;

                //Compare new CarTypeDetails with existing CarTypeDetails   
                if (companyDTO.CarTypeIds.Contains(0))
                {
                    companyDTO.CarTypeIds.Clear();
                }
                var isDetailsChange = await IsCompanyDetailChange(companyDTO.CarTypeIds, existingCompany.Id);

                if (isDetailsChange)
                {
                    await UpdateCompanyDetails(existingCompany.Id, companyDTO.CarTypeIds);
                    EditHelper<Company>.SetModifiedIfNecessary(company, true, existingCompany, "NewUserId");
                }
                else
                {
                    bool isValueChange = EditHelper<Company>
                                            .HasChanges(company, existingCompany);//Check if Company data changed
                    EditHelper<Company>.SetModifiedIfNecessary(company, isValueChange, existingCompany, "NewUserId");
                }
                await _companyRepository.UpdateAsync(company);
                return new OperationResult(true, "Company update succesfully", StatusCodes.Status200OK);
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

        private async Task UpdateCompanyDetails(int companyId, List<int> carTypeIds)
        {
            await _carTypeDetailRepository.ClearCompanyDetailsAsync(companyId);
            await _carTypeDetailRepository.AddCarTypesListAsync(companyId, carTypeIds);
        }

        private async Task<bool> IsCompanyDetailChange(List<int> selectedCarTypes, int existingCompanyId)
        {
            var previousDetails = await _carTypeDetailRepository.GetCompanyDetails(existingCompanyId);
            var carTypes = await _carTypeRepository.GetAllAsync();
            foreach (var carType in carTypes)
            {
                bool previousChecked = previousDetails != null && previousDetails.Any(c => c.CarType.Id.Equals(carType.Id));
                bool currentChecked = selectedCarTypes.Contains(carType.Id);
                if (previousChecked != currentChecked)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
