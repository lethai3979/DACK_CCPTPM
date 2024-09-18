using AutoMapper;
using GoWheels_WebAPI.Models.DTOs;
using GoWheels_WebAPI.Models.DTOs.SalePromotionDTOs;
using GoWheels_WebAPI.Models.Entities;

namespace GoWheels_WebAPI.Mapping
{
    public class MappingProfile : Profile 
    {
        public MappingProfile() 
        {
            //For Mapping
            CreateMap<AmenityDTO, Amentity>();
            CreateMap<Amentity, AmenityDTO>();
            CreateMap<CarType, CarTypeDTO>();
            CreateMap<CarTypeDTO, CarType>();
            CreateMap<CompanyDTO, Company>();
            CreateMap<Company, CompanyDTO>();
            CreateMap<Promotion, PromotionType>();
            CreateMap<PromotionType, Promotion>();
            CreateMap<SalePromotionDto, Promotion>().ReverseMap();
            CreateMap<CarTypeDetailDTO, CarTypeDetail>();
            CreateMap<CarTypeDetail, CarTypeDetailDTO>();

            //For Update 
            CreateMap<CarType, CarType>();
            CreateMap<Amentity, Amentity>();
            CreateMap<Company, Company>();
            CreateMap<PromotionType, PromotionType>();
            CreateMap<Promotion, Promotion>();
        }
    }
}
