using AutoMapper;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.Entities;
using GoWheels_WebAPI.Models.ViewModels;

namespace GoWheels_WebAPI.Mapping
{
    public class MappingProfile : Profile 
    {
        public MappingProfile() 
        {
            //View Model
            CreateMap<AmenityVM, Amenity>().ReverseMap();
            CreateMap<CarTypeVM, CarType>().ReverseMap();
            CreateMap<CompanyVM, Company>().ReverseMap();
            CreateMap<Amenity,AmenityVM>().ReverseMap();
            CreateMap<PromotionVM, Promotion>().ReverseMap();
            CreateMap<CarTypeDetailVM, CarTypeDetail>().ReverseMap();
            CreateMap<Promotion, PromotionVM>().ReverseMap();
            CreateMap<PostPromotion, PostPromotionVM>().ReverseMap();
            CreateMap<Post, PostVM>().ReverseMap()
                .ForMember(dest => dest.CarType, opt => opt.Ignore())
                .ForMember(dest => dest.Company, opt => opt.Ignore());
            CreateMap<ApplicationUser, UserVM>().ReverseMap();
            CreateMap<PostImage, PostImageVM>().ReverseMap();
            CreateMap<PostAmenity, PostAmenityVM>().ReverseMap();
            CreateMap<Rating, RatingVM>().ReverseMap();
            CreateMap<ReportType, ReportTypeVM>().ReverseMap();
            CreateMap<Report, ReportVM>().ReverseMap();
            CreateMap<Favorite, FavoriteVM>().ReverseMap();
            CreateMap<Booking, BookingVM>().ReverseMap();
            CreateMap<Invoice, InvoiceVM>().ReverseMap();
            CreateMap<Driver, DriverVM>().ReverseMap();
            CreateMap<DriverBooking, DriverBookingVM>().ReverseMap();   


            //DTOs
            CreateMap<Amenity, AmenityDTO>().ReverseMap();
            CreateMap<CarType, CarTypeDTO>().ReverseMap();
            CreateMap<Company, CompanyDTO>().ReverseMap();
            CreateMap<Post, PostDTO>().ReverseMap();
            CreateMap<Promotion, PromotionDTO>().ReverseMap();
            CreateMap<Rating, RatingDTO>().ReverseMap();
            CreateMap<ReportType, ReportTypeDTO>().ReverseMap();
            CreateMap<Report, ReportDTO>().ReverseMap();
            CreateMap<Favorite, FavoriteDTO>().ReverseMap();
            CreateMap<Booking, BookingDTO>().ReverseMap();
            CreateMap<UserDTO, ApplicationUser>().ReverseMap();

        }
    }
}
