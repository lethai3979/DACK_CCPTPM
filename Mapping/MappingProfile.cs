using AutoMapper;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.DTOs.PostDTOs;
using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Mapping
{
    public class MappingProfile : Profile 
    {
        public MappingProfile() 
        {
            //For Mapping
            CreateMap<AmenityDTO, Amenity>().ReverseMap();
            CreateMap<CarTypeDTO, CarType>().ReverseMap();
            CreateMap<CarType, AddCarTypeDTO>().ReverseMap();
            CreateMap<CompanyDTO, Company>().ReverseMap();
            CreateMap<Amenity,AmenityDTO>().ReverseMap();
            CreateMap<SalePromotionDTO, Promotion>().ReverseMap();
            CreateMap<CarTypeDetailDTO, CarTypeDetail>().ReverseMap();
            CreateMap<PromotionType, SalePromotionTypeDTO>().ReverseMap();
            CreateMap<Promotion, SalePromotionDTO>().ReverseMap();
<<<<<<< HEAD
            CreateMap<Post, PostDTO>().ReverseMap()
                .ForMember(dest => dest.CarType, opt => opt.Ignore()) // Ánh xạ bằng tay nếu cần
                .ForMember(dest => dest.Company, opt => opt.Ignore());
            CreateMap<PostAmenity, PostAmenityDTO>().ReverseMap();

=======
            CreateMap<Rating, RatingDTO>().ReverseMap();

            //For Update 
            CreateMap<CarType, CarType>();
            CreateMap<Amenity, Amenity>();
            CreateMap<Company, Company>();
            CreateMap<PromotionType, PromotionType>();
            CreateMap<Promotion, Promotion>();
            CreateMap<Rating, Rating>();
>>>>>>> origin/NguyenThanhKy

        }
    }
}
