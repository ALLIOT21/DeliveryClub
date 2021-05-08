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
            CreateMap<Admin, AdminDAO>()
                .ReverseMap();

            CreateMap<Restaurant, RestaurantDAO>()
                .ForMember(m => m.Specializations, act => act.Ignore());

            CreateMap<RestaurantDAO, Restaurant>()
                .ForMember(m => m.Specializations, act => act.Ignore());

            CreateMap<RestaurantAdditionalInfoDAO, RestaurantAdditionalInfo>().
                ReverseMap();

            CreateMap<PortionPrice, PortionPriceDAO>()
                .ReverseMap();

            CreateMap<PortionPriceProductGroup, PortionPriceProductGroupsDAO>()
                .ReverseMap();

            CreateMap<ProductGroup, ProductGroupDAO>()
                .ReverseMap();

            CreateMap<Product, ProductDAO>()
                .ReverseMap();

            CreateMap<PortionPriceProduct, PortionPriceProductsDAO>()
                .ReverseMap();

            CreateMap<Dispatcher, DispatcherDAO>()
                .ReverseMap();

            CreateMap<Order, OrderDAO>()
                .ReverseMap();

            CreateMap<RestaurantOrder, RestaurantOrderDAO>()
                .ReverseMap();

            CreateMap<OrderedProduct, OrderedProductDAO>()
                .ReverseMap();
        }
    }
}
