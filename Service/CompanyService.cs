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
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public CompanyService(CompanyRepository companyRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _companyRepository = companyRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OperationResult> GetByIdAsync(int id)
        {
            var company = await _companyRepository.GetByIdAsync(id);
            if (company == null)
            {
                return new OperationResult(false, "Company not found", StatusCodes.Status404NotFound);
            }
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
            companyDTO.CreateById = _httpContextAccessor.HttpContext?.User?
                                    .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";//Get user Id
            companyDTO.CreateOn = DateTime.Now;
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
    }
}
