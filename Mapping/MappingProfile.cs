using AutoMapper;
using GoWheels_WebAPI.Models.DTOs;
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
            CreateMap<CompanyDTO, Company>().ReverseMap();
            CreateMap<SalePromotionDTO, Promotion>().ReverseMap();
            CreateMap<CarTypeDetailDTO, CarTypeDetail>().ReverseMap();
            CreateMap<PromotionType, SalePromotionTypeDTO>().ReverseMap();
            CreateMap<Promotion, SalePromotionDTO>().ReverseMap();

            //For Update 
            CreateMap<CarType, CarType>();
            CreateMap<Amenity, Amenity>();
            CreateMap<Company, Company>();
            CreateMap<PromotionType, PromotionType>();
            CreateMap<Promotion, Promotion>();

        }
    }
}
