using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Repositories.Interface;
using GoWheels_WebAPI.Service.Interface;
using GoWheels_WebAPI.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GoWheels_WebAPI.Service
{
    public class CompanyService : ICompanyService   
    {
        private readonly IGenericRepository<Company> _companyRepository;
        private readonly ICarTypeDetailRepository _carTypeDetailRepository;
        private readonly IGenericRepository<CarType> _carTypeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _userId;

        public CompanyService(IGenericRepository<Company> companyRepository, 
                            IHttpContextAccessor httpContextAccessor,
                            ICarTypeDetailRepository carTypeDetailRepository,
                            IGenericRepository<CarType> carTypeRepository)
        {
            _companyRepository = companyRepository;
            _httpContextAccessor = httpContextAccessor;
            _carTypeDetailRepository = carTypeDetailRepository;
            _carTypeRepository = carTypeRepository;
            _userId = _httpContextAccessor.HttpContext?.User?
                        .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
        }

        public List<Company> GetAll()
            => _companyRepository.GetAll();


        public Company GetById(int id)
            => _companyRepository.GetById(id);


        public void Add(Company company, List<int> carTypeIds, IFormFile formFile)
        {

            try
            {
                if (formFile != null && formFile.Length > 0)
                {
                    company.IconImage = SaveImage(formFile);
                }
                if (carTypeIds.Contains(0))
                {
                    throw new InvalidOperationException("Invalid car type Id");
                }
                company.CreatedById = _userId;
                company.CreatedOn = DateTime.Now;
                company.IsDeleted = false;
                _companyRepository.Add(company);
                _carTypeDetailRepository.AddCarTypesList(company.Id, carTypeIds);
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
        private string SaveImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File cannot be null or empty");
            }

            // Đường dẫn tới thư mục lưu trữ ảnh
            var savePath = "./wwwroot/images/companies/";
            var fileName = Path.GetFileName(file.FileName); // Đặt tên ngẫu nhiên để tránh trùng lặp
            var filePath = Path.Combine(savePath, fileName);

            try
            {
                // Tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                // Lưu ảnh vào thư mục
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                // Trả về URL để lưu vào database
                return "images/companies/" + fileName;
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                throw new Exception("Could not save file", ex);
            }
        }
        public void DeleteById(int id)
        {
            try
            {
                var company = _companyRepository.GetById(id);
                company.ModifiedById = _userId;
                company.ModifiedOn = DateTime.Now;
                company.IsDeleted = !company.IsDeleted;
                _companyRepository.Update(company);
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

        public void Update(int id, Company company, List<int> carTypeIds, IFormFile formFile)
        {
            try
            {
                string imageUrl = "";
                if (company.IconImage != null)
                {
                    imageUrl = "./wwwroot/images/companies/" + Path.GetFileName(formFile.FileName);
                }
                var existingCompany = _companyRepository.GetById(id);

                if (formFile != null && formFile.Length > 0 && imageUrl != existingCompany.IconImage)
                {
                    company.IconImage = SaveImage(formFile);
                }
                else
                {
                    company.IconImage = existingCompany.IconImage;
                }
                company.CreatedOn = existingCompany.CreatedOn;
                company.CreatedById = existingCompany.CreatedById;
                company.ModifiedById = existingCompany.ModifiedById;
                company.ModifiedOn = existingCompany.ModifiedOn;

                //Compare new CarTypeDetails with existing CarTypeDetails   
                if (carTypeIds.Contains(0))
                {
                    throw new InvalidOperationException("Invalid car type Id");
                }
                var isDetailsChange = IsCompanyDetailChange(carTypeIds, existingCompany.Id);

                if (isDetailsChange)
                {
                    UpdateCompanyDetails(existingCompany.Id, carTypeIds);
                    EditHelper<Company>.SetModifiedIfNecessary(company, true, existingCompany, _userId);
                }
                else
                {
                    bool isValueChange = EditHelper<Company>
                                            .HasChanges(company, existingCompany);//Check if Company data changed
                    EditHelper<Company>.SetModifiedIfNecessary(company, isValueChange, existingCompany, _userId);
                }
                _companyRepository.Update(company);
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

        private void UpdateCompanyDetails(int companyId, List<int> carTypeIds)
        {
            _carTypeDetailRepository.ClearCompanyDetails(companyId);
            _carTypeDetailRepository.AddCarTypesList(companyId, carTypeIds);
        }

        private bool IsCompanyDetailChange(List<int> selectedCarTypes, int existingCompanyId)
        {
            var previousDetails = _carTypeDetailRepository.GetCompanyDetails(existingCompanyId);
            var carTypes = _carTypeRepository.GetAll();
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
