using AutoMapper;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
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
        private readonly string _userId;

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
            _userId = _httpContextAccessor.HttpContext?.User?
                        .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
        }

        public async Task<List<Company>> GetAllAsync()
        {
            var companies = await _companyRepository.GetAllAsync();
            if(companies.Count == 0)
            {
                throw new NullReferenceException("List is empty");
            }
            return companies;
        }

        public async Task<Company> GetByIdAsync(int id)
            => await _companyRepository.GetByIdAsync(id);


        public async Task AddAsync(Company company, List<int> carTypeIds)
        {

            try
            {
                if (carTypeIds.Contains(0) || carTypeIds.Count == 0)
                {
                    carTypeIds.Clear();
                }
                company.CreatedById = _userId;
                company.CreatedOn = DateTime.Now;
                company.IsDeleted = false;
                await _companyRepository.AddAsync(company);
                await _companyRepository.AddCompanyDetailAsync(company.Id, carTypeIds);
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
                var company = await _companyRepository.GetByIdAsync(id);
                company.ModifiedById = _userId;
                company.ModifiedOn = DateTime.Now;
                company.IsDeleted = !company.IsDeleted;
                await _companyRepository.UpdateAsync(company);
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

        public async Task UpdateAsync(int id, Company company, List<int> carTypeIds)
        {
            try
            {
                //Check if Company exist
                var existingCompany = await _companyRepository.GetByIdAsync(id);
                company.CreatedOn = existingCompany.CreatedOn;
                company.CreatedById = existingCompany.CreatedById;
                company.ModifiedById = existingCompany.ModifiedById;
                company.ModifiedOn = existingCompany.ModifiedOn;

                //Compare new CarTypeDetails with existing CarTypeDetails   
                if (carTypeIds.Contains(0) || carTypeIds.Count == 0)
                {
                    carTypeIds.Clear();
                }
                var isDetailsChange = await IsCompanyDetailChange(carTypeIds, existingCompany.Id);

                if (isDetailsChange)
                {
                    await UpdateCompanyDetails(existingCompany.Id, carTypeIds);
                    EditHelper<Company>.SetModifiedIfNecessary(company, true, existingCompany, _userId);
                }
                else
                {
                    bool isValueChange = EditHelper<Company>
                                            .HasChanges(company, existingCompany);//Check if Company data changed
                    EditHelper<Company>.SetModifiedIfNecessary(company, isValueChange, existingCompany, _userId);
                }
                await _companyRepository.UpdateAsync(company);
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
