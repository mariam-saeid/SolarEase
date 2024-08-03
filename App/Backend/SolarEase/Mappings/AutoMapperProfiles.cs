using AutoMapper;
using SolarEase.Models.Domain;
using SolarEase.Models.DTO.AdminDto;
using SolarEase.Models.DTO.CalculatorDto;
using SolarEase.Models.DTO.EnergyPredictionDto;
using SolarEase.Models.DTO.FAQCategoryDto;
using SolarEase.Models.DTO.FavoritePostDto;
using SolarEase.Models.DTO.FAQDto;
using SolarEase.Models.DTO.PeakHourDto;
using SolarEase.Models.DTO.PersonDto;
using SolarEase.Models.DTO.ProductCategoryDto;
using SolarEase.Models.DTO.ProfileDto;
using SolarEase.Models.DTO.SolarInstallerDto;
using SolarEase.Models.DTO.SolarProductDto;
using SolarEase.Models.DTO.MessageDto;

namespace SolarEase.Mapper
{
    public class AutoMapperProfiles : AutoMapper.Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<SolarInstaller, SolarInstallerDto>().ReverseMap();
            CreateMap<SolarInstaller, CreateSolarInstallerDto>().ReverseMap();
            CreateMap<SolarProduct, CreateSolarProductDto>().ReverseMap();
            CreateMap<ProductCategory, CreateProductCategoryDto>().ReverseMap();
            CreateMap<ProductCategory, ProductCategoryDto>().ReverseMap();
            CreateMap<FavoritePost, CreateFavoritePostDto>().ReverseMap();
            CreateMap<Models.Domain.Profile, ProfileDto>().ReverseMap();
            CreateMap<PeakHour, PeakHourDto>().ReverseMap();
            CreateMap<ElectricityConsumption, ElectricityConsumptionDto>().ReverseMap();
            CreateMap<FAQCategory, FAQCategoryDto>().ReverseMap();
            CreateMap<FAQCategory, CreateFAQCategoryDto>().ReverseMap();
            CreateMap<FAQ, CreateFAQDto>().ReverseMap();
            CreateMap<Message, CreateMessageDto>().ReverseMap();

        }
    }
}
