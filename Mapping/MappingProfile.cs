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
            CreateMap<AmenityDTO, Amenity>();
            CreateMap<Amenity, AmenityDTO>();
            CreateMap<CarType, CarTypeDTO>();
            CreateMap<CarTypeDTO, CarType>();
            CreateMap<CompanyDTO, Company>();
            CreateMap<Company, CompanyDTO>();
            CreateMap<Promotion, PromotionType>();
            CreateMap<PromotionType, Promotion>();

            //For Update 
            CreateMap<CarType, CarType>();
            CreateMap<Amenity, Amenity>();
            CreateMap<Company, Company>();
            CreateMap<PromotionType, PromotionType>();
            CreateMap<Promotion, Promotion>();
        }
    }
}
