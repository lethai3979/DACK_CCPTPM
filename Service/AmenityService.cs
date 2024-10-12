using AutoMapper;
using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Repositories;
using GoWheels_WebAPI.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace GoWheels_WebAPI.Service
{
    public class AmenityService
    {
        public readonly AmenityRepository _amenityRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _userId;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AmenityService(AmenityRepository amenityRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment webHostEnvironment)
        {
            _amenityRepository = amenityRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userId = _httpContextAccessor.HttpContext?.User?
                        .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
            _webHostEnvironment = webHostEnvironment;
        }   

        public async Task<OperationResult> GetAllAsync()
        {
            var amenityList = await _amenityRepository.GetAllAsync();
            if (amenityList.Count != 0)
            {
                var amenityListVM = _mapper.Map<List<AmenityVM>>(amenityList);
                return new OperationResult(true, statusCode: StatusCodes.Status200OK, data: amenityListVM);
            }
            return new OperationResult(message: "List empty", statusCode: StatusCodes.Status204NoContent);
        }

        public async Task<OperationResult> GetByIdAsync(int id)
        {
            try
            {
                var amenity = await _amenityRepository.GetByIdAsync(id);
                if (amenity != null)
                {
                    var amenityVM = _mapper.Map<AmenityVM>(amenity);
                    return new OperationResult(true, "Amenity found", StatusCodes.Status200OK, amenityVM);
                }
                return new OperationResult(false, "Amenity not found", StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return new OperationResult(false, $"An error occurred: {ex.Message}", StatusCodes.Status500InternalServerError);
            }
        }

        public async Task<OperationResult> AddAsync(AmenityDTO amenityDTO)
        {
            try
            {
                // Bước 1: Lưu tệp ảnh và lấy URL nếu có ảnh

                var amenity = _mapper.Map<Amenity>(amenityDTO);
                amenity.CreatedById = _userId;
                amenity.CreatedOn = DateTime.Now;
                amenity.IsDeleted = false;


                // Bước 3: Lưu tiện nghi vào cơ sở dữ liệu
                await _amenityRepository.AddAsync(amenity);

                // Bước 4: Trả về kết quả thành công
                return new OperationResult(true, "Amenity added successfully", StatusCodes.Status200OK);
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

        public async Task<OperationResult> AddAsync(AmenityDTO amenityDTO)
        {
            try
            {
                // Bước 1: Lưu tệp ảnh và lấy URL nếu có ảnh
                string imageUrl = null;
                if (amenityDTO.IconImage != null && amenityDTO.IconImage.Length > 0)
                {
                    imageUrl = await SaveImage(amenityDTO.IconImage);
                }
                // Bước 1: Map DTO sang entity
                var amenity = _mapper.Map<Amenity>(amenityDTO);
                // Bước 2: Thêm thông tin metadata
                amenity.CreatedById = _userId;
                amenity.CreatedOn = DateTime.Now;
                amenity.IsDeleted = false;
                amenity.IconImage = imageUrl;
                // Bước 3: Lưu tiện nghi vào cơ sở dữ liệu
                await _amenityRepository.AddAsync(amenity);

                // Bước 4: Trả về kết quả thành công
                return new OperationResult(true, "Amenity added successfully", StatusCodes.Status200OK);
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

        public async Task<string> SaveImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File cannot be null or empty");
            }

            // Đường dẫn tới thư mục lưu trữ ảnh
            var savePath = "./wwwroot/images/amenities/";
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName); // Đặt tên ngẫu nhiên để tránh trùng lặp
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
                    await file.CopyToAsync(fileStream);
                }

                // Trả về URL để lưu vào database
                return "https://localhost:7265/images/amenities/" + fileName;
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                throw new Exception("Could not save file", ex);
            }
        }





        public async Task<OperationResult> DeletedByIdAsync(int id)
        {
            try
            {
                var amenity = await _amenityRepository.GetByIdAsync(id);
                if (amenity == null)
                {
                    return new OperationResult(false, "Amenity not found", StatusCodes.Status404NotFound);
                }
                amenity.ModifiedById = _userId;
                amenity.ModifiedOn = DateTime.Now;
                amenity.IsDeleted = true;
                await _amenityRepository.UpdateAsync(amenity);
                return new OperationResult(true, "Amenity deleted succesfully", StatusCodes.Status200OK);
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
        public async Task<OperationResult> UpdateAsync(int id, AmenityDTO amenityDTO)
        {
            string imageUrl = null;
            try
            {
                var existingAmenity = await _amenityRepository.GetByIdAsync(id);
                if (amenityDTO.IconImage != null && amenityDTO.IconImage.Length > 0)
                {
                    imageUrl = await SaveImage(amenityDTO.IconImage);
                }
                
                if (existingAmenity == null)
                {
                    return new OperationResult(true, "Amenity not found", StatusCodes.Status404NotFound);
                }
                var amenity = _mapper.Map<Amenity>(amenityDTO);

                if (imageUrl != null)
                {
                    amenity.IconImage = imageUrl;
                }
                else
                {
                    amenity.IconImage = existingAmenity.IconImage;
                }
                amenity.CreatedOn = existingAmenity.CreatedOn;
                amenity.CreatedById = existingAmenity.CreatedById;
                amenity.ModifiedById = existingAmenity.ModifiedById;
                amenity.ModifiedOn = existingAmenity.ModifiedOn;
                var isValueChange = EditHelper<Amenity>.HasChanges(amenity, existingAmenity);
                EditHelper<Amenity>.SetModifiedIfNecessary(amenity, isValueChange, existingAmenity, _userId);
                await _amenityRepository.UpdateAsync(amenity);
                return new OperationResult(true, "Amenity update succesfully", StatusCodes.Status200OK);
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
