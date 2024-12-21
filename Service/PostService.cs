using GoWheels_WebAPI.Data;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;
using GoWheels_WebAPI.Repositories.Interface;
using GoWheels_WebAPI.Service.Interface;
using GoWheels_WebAPI.Utilities;
using System.Security.Claims;

namespace GoWheels_WebAPI.Service
{
    public class PostService : IPostService
    {

        private readonly IPostRepository _postRepository;
        private readonly IPostAmenityRepository _postAmenityRepository;
        private readonly IAmenityService _amenityService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;
        private  readonly string _userId;

        public PostService(IPostRepository postRepository,
                            IPostAmenityRepository postAmenityRepository,
                            IAmenityService amenityService,
                            IHttpContextAccessor httpContextAccessor,
                            ApplicationDbContext context)
        {
            _postRepository = postRepository;
            _postAmenityRepository = postAmenityRepository;
            _amenityService = amenityService;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _userId = _httpContextAccessor.HttpContext?.User?
                        .FindFirstValue(ClaimTypes.NameIdentifier) ?? "UnknownUser";
        }

        public List<Post> GetAll()
            => _postRepository.GetAll();

        public Post GetById(int id)
            => _postRepository.GetById(id);

        public List<Post> GetAllByUserId()
            => _postRepository.GetPostsByUserId(_userId);

        public List<Post> GetAllByUserId(string userId)
            => _postRepository.GetPostsByUserId(userId);

        public List<Post> ApplyFilters(List<Post> query, SearchFilterModel filterModel)
        {
            if (!string.IsNullOrWhiteSpace(filterModel.Company))
            {
                query = query.Where(post => post.Company.Name == filterModel.Company).ToList();
            }

            if (!string.IsNullOrEmpty(filterModel.CarType))
            {
                query = query.Where(post => post.CarType.Name == filterModel.Company).ToList();
            }

            if (filterModel.Seat > 0)
            {
                query = query.Where(post => post.Seat == filterModel.Seat).ToList();
            }

            if (!string.IsNullOrWhiteSpace(filterModel.Gear))
            {
                query = filterModel.Gear switch
                {
                    "Số sàn" => query.Where(post => !post.Gear).ToList(),
                    _ => query.Where(post => post.Gear).ToList()
                };
            }

            if (!string.IsNullOrWhiteSpace(filterModel.Fuel))
            {
                query = filterModel.Fuel switch
                {
                    "Xăng" => query.Where(post => post.Fuel == "Xăng").ToList(),
                    "Điện" => query.Where(post => post.Fuel == "Điện").ToList(),
                    _ => query.Where(post => post.Fuel == "Dầu").ToList()
                };
            }

            if (filterModel.HasDriver)
            {
                query = query.Where(post => post.HasDriver).ToList();
            }

            return query;
        }

        public void Add(Post post, IFormFile formFile, List<IFormFile> formFiles, List<int> amenitiesIds)
        {
            try
            {

                List<string> imgUrls = new List<string>();
                if (formFile != null && formFile.Length > 0)
                {
                    post.Image = SaveImage(formFile);
                }
                post.CreatedById = _userId;
                post.CreatedOn = DateTime.Now;
                post.UserId = _userId;
                post.AvgRating = 0;
                post.IsDeleted = false;
                post.IsDisabled = false;
                post.IsHidden = false;
                _postRepository.Add(post);
                if (formFiles.Count != 0)
                {
                    foreach (var file in formFiles)
                    {
                        var img = SaveImage(file);
                        imgUrls.Add(img);
                    }

                    _postRepository.AddPostImages(imgUrls, post.Id);
                }
                if (amenitiesIds.Count != 0)
                {
                    _postAmenityRepository.AddRange(amenitiesIds, post.Id);
                }
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
            var savePath = "./wwwroot/images/posts/";
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

                return "images/posts/" + fileName;
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                throw new Exception("Could not save file", ex);
            }
        }

        private bool IsPostAmenityChange(List<int> selectedAmenities, int existingPostId)
        {
            var previousDetails = _postAmenityRepository.GetAmenityByPostId(existingPostId);
            var amenities = _amenityService.GetAll();
            foreach (var amenity in amenities)
            {
                bool previousChecked = previousDetails != null && previousDetails.Any(c => c.AmenityId.Equals(amenity.Id));
                bool currentChecked = selectedAmenities.Contains(amenity.Id);
                if (previousChecked != currentChecked)
                {
                    return true;
                }
            }
            return false;
        }

        private void UpdatePostAmenities(int postId, List<int> amenitiesIds)
        {
            _postAmenityRepository.RemoveRange(postId);
            _postAmenityRepository.AddRange(amenitiesIds, postId);
        }

        public void Update(int id, Post post, IFormFile image, List<int> amenitiesIds)
        {
            try
            {
                var existingPost = _postRepository.GetById(id);
                var imageUrl = "./wwwroot/images/posts/" + Path.GetFileName(image.FileName);
                if (_userId != existingPost.UserId)
                {
                    throw new UnauthorizedAccessException("Unauthorize");
                }
                if (post.Image == null)
                {
                    post.Image = existingPost.Image;
                }
                else
                {
                    post.Image = SaveImage(image);
                }
                post.CreatedOn = existingPost.CreatedOn;
                post.CreatedById = existingPost.CreatedById;
                post.ModifiedById = existingPost.ModifiedById;
                post.ModifiedOn = existingPost.ModifiedOn;
                post.AvgRating = existingPost.AvgRating;
                post.IsDeleted = existingPost.IsDeleted;
                post.Favorites = existingPost.Favorites;
                if (image != null && imageUrl != existingPost.Image)
                {
                    post.Image = SaveImage(image);
                }
                else
                {
                    post.Image = existingPost.Image;
                }
                post.Images = existingPost.Images;
                post.UserId = existingPost.UserId;
                post.User = existingPost.User;
                post.CarType = existingPost.CarType;
                post.Company = existingPost.Company;
                post.Ratings = existingPost.Ratings;
                post.Reports = existingPost.Reports;
                if (amenitiesIds.Contains(0) || amenitiesIds.Count == 0)
                {
                    amenitiesIds.Clear();
                }
                var isPostAmenitiesChange = IsPostAmenityChange(amenitiesIds, existingPost.Id);
                if (isPostAmenitiesChange)
                {
                    UpdatePostAmenities(existingPost.Id, amenitiesIds);
                    EditHelper<Post>.SetModifiedIfNecessary(post, true, existingPost, _userId);
                }
                else
                {
                    var isPostDataChange = EditHelper<Post>.HasChanges(post, existingPost);
                    EditHelper<Post>.SetModifiedIfNecessary(post, isPostDataChange, existingPost, _userId);
                }
                _postRepository.Update(post);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private bool IsImagesListChange(List<IFormFile> imageList, Post post)
        {
            foreach (var file in imageList)
            {
                var url = "images/posts/" + Path.GetFileName(file.FileName);
                var isChange = !post.Images.Any(i => i.Url.Equals(url));
                if (isChange)
                {
                    return true;
                }
            }
            return false;
        }
        public void UpdatePostImages(List<IFormFile> imageFiles, int postId)
        {
            try
            {
                var post = _postRepository.GetById(postId);
                if (_userId != post.UserId)
                {
                    throw new UnauthorizedAccessException("Unauthorize");
                }
                var imageUrls = new List<string>();
                if (imageFiles.Count != 0)
                {
                    var isImagesChange = IsImagesListChange(imageFiles, post);
                    if (!isImagesChange)
                    {
                        return;
                    }
                    post.ModifiedById = _userId;
                    post.ModifiedOn = DateTime.Now;
                    _postRepository.Update(post);
                    _postRepository.DeletePostImages(post.Id);
                    foreach (var file in imageFiles)
                    {
                        var img = SaveImage(file);
                        imageUrls.Add(img);
                    }
                    _postRepository.AddPostImages(imageUrls, postId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdatePostAverageRating(int postId, float avgRating)
        {
            try
            {
                var post = _postRepository.GetById(postId);
                if (post == null) throw new InvalidOperationException("Post not found");
                post.AvgRating = avgRating;
                _postRepository.Update(post);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateRideNumber(int postId, int rideNumber)
        {
            try
            {
                var post = _postRepository.GetById(postId);
                post.RideNumber += rideNumber;
                _postRepository.Update(post);
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
                var post = _postRepository.GetById(id);
                if (_userId != post.UserId)
                {
                    throw new UnauthorizedAccessException("Unauthorize");
                }
                post.IsDeleted = true;
                post.ModifiedById = _userId;
                post.ModifiedOn = DateTime.Now;
                _postRepository.Update(post);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DisablePostById(int id)
        {
            try
            {
                var post = _postRepository.GetById(id);
                post.IsDisabled = true;
                post.ModifiedById = _userId;
                post.ModifiedOn = DateTime.Now;
                _postRepository.Update(post);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
