using AutoMapper;
using DeliveryClub.Data.DTO.ActorsDTO;
using DeliveryClub.Data.DTO.EntitiesDTO;
using DeliveryClub.Domain.Models.Actors;
using DeliveryClub.Domain.Models.Entities;

namespace DeliveryClub.Domain.Logic.Mapping
{
    public class DomainMappingProfile : Profile
    {
        public DomainMappingProfile()
        {
            CreateMap<Admin, AdminDTO>()
                .ReverseMap();

            CreateMap<Restaurant, RestaurantDTO>()
                .ForMember(m => m.Specializations, act => act.Ignore());

            CreateMap<RestaurantDTO, Restaurant>()
                .ForMember(m => m.Specializations, act => act.Ignore());

            CreateMap<RestaurantAdditionalInfo, RestaurantAdditionalInfoDTO>()
                .ForMember(m => m.PaymentMethods, act => act.Ignore());

            CreateMap<RestaurantAdditionalInfoDTO, RestaurantAdditionalInfo>()
                .ForMember(m => m.PaymentMethods, act => act.Ignore());

            CreateMap<PortionPrice, PortionPriceDTO>()
                .ReverseMap();

            CreateMap<PortionPriceProductGroup, PortionPriceProductGroupsDTO>()
                .ReverseMap();

            CreateMap<ProductGroup, ProductGroupDTO>()
                .ReverseMap();
        }
    }
}
