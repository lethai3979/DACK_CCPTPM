using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories.Interface;
using GoWheels_WebAPI.Service.Interface;
using GoWheels_WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GoWheels_WebAPI.Service
{
    public class CarTypeService : ICarTypeService
    {
        private readonly IGenericRepository<CarType> _carTypeRepository;
        private readonly IGenericRepository<Company> _companyRepository;
        private readonly ICarTypeDetailRepository _carTypeDetailRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _userId;

        public CarTypeService(IGenericRepository<CarType> carTypeRepository,
                            IGenericRepository<Company> companyRepository,
                            ICarTypeDetailRepository carTypeDetailRepository,
                            IHttpContextAccessor httpContextAccessor)
    {
            _carTypeRepository = carTypeRepository;
            _httpContextAccessor = httpContextAccessor;
            _companyRepository = companyRepository;
            _carTypeDetailRepository = carTypeDetailRepository;
            _userId = _httpContextAccessor.HttpContext?.User?
                        .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
        }

        public List<CarType> GetAll()
            => _carTypeRepository.GetAll();


        public CarType GetById(int id)
            => _carTypeRepository.GetById(id);

        public void Add(CarType carType, List<int> companyList)
        {
            try
            {
                if (companyList.Contains(0))
                {
                    throw new InvalidOperationException("Invalid company Id");
                }
                carType.CreatedById = _userId;
                carType.CreatedOn = DateTime.Now;
                carType.IsDeleted = false;
                _carTypeRepository.Add(carType);
                _carTypeDetailRepository.AddCompaniesList(carType.Id, companyList);
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

        public void DeleteById(int id)
        {
            try
            {
                var carType = _carTypeRepository.GetById(id);
                carType.ModifiedById = _userId;
                carType.ModifiedOn = DateTime.Now;
                carType.IsDeleted = !carType.IsDeleted;
                _carTypeRepository.Update(carType);
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

        private bool IsCarTypeDetailChange(List<int> selectedCompanies, int existingCarTypeId)
        {
            var previousDetails = _carTypeDetailRepository.GetCarTypeDetails(existingCarTypeId);
            var companies = _companyRepository.GetAll();
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

        private void UpdateCarTypeDetails(int carTypeId, List<int> companyIds)
        {
            _carTypeDetailRepository.ClearCarTypeDetails(carTypeId);//Clear previous CarTypeDetails
            _carTypeDetailRepository.AddCompaniesList(carTypeId, companyIds);//Add new CarTypeDetails
        }

        public void Update(int id, CarType carType, List<int> companyIds)
        {

            try
            {
                var existingCarType = _carTypeRepository.GetById(id);
                carType.CreatedById = existingCarType.CreatedById;
                carType.CreatedOn = existingCarType.CreatedOn;
                carType.ModifiedById = existingCarType.ModifiedById;
                carType.ModifiedOn = existingCarType.ModifiedOn;
                if (companyIds.Contains(0))
                {
                    throw new InvalidOperationException("Invalid company Id");
                }
                //Compare new CarTypeDetails with existing CarTypeDetails
                var isDetailsChange = IsCarTypeDetailChange(companyIds, existingCarType.Id);
                if (isDetailsChange)
                {

                    UpdateCarTypeDetails(existingCarType.Id, companyIds);
                    EditHelper<CarType>.SetModifiedIfNecessary(carType, true, existingCarType, _userId);
                }
                else
                {
                    bool isCarTypeDataChange = EditHelper<CarType>
                                            .HasChanges(carType, existingCarType);//Check if CarType data changed
                    EditHelper<CarType>.SetModifiedIfNecessary(carType, isCarTypeDataChange, existingCarType, _userId);
                }
                _carTypeRepository.Update(carType);
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
