using AutoMapper;
using DeliveryClub.Domain.AuxiliaryModels.SuperUser;
using DeliveryClub.Web.ViewModels.SuperUser;
using DeliveryClub.Web.ViewModels.Admin;
using DeliveryClub.Domain.AuxiliaryModels.Admin;
using DeliveryClub.Web.ViewModels;
using DeliveryClub.Web.ViewModels.Admin.Dispatchers;
using DeliveryClub.Web.ViewModels.Admin.Menu;
using DeliveryClub.Web.ViewModels.Admin.Info;
using DeliveryClub.Web.ViewModels.Guest;
using DeliveryClub.Domain.AuxiliaryModels.Guest;

namespace DeliveryClub.Web.Mapping
{
    public class ViewMappingProfile : Profile
    {
        public ViewMappingProfile()
        {
            CreateMap<CreateAdminViewModel, CreateAdminModel>()
                .ReverseMap();

            CreateMap<GetAdminViewModel, GetAdminModel>()
                .ReverseMap();

            CreateMap<UpdateAdminViewModel, UpdateAdminModel>()
                .ReverseMap();

            CreateMap<RestaurantInfoViewModel, RestaurantInfoModel>()
                .ReverseMap();

            CreateMap<SpecializationViewModel, SpecializationModel>()
                .ReverseMap();

            CreateMap<PaymentMethodViewModel, PaymentMethodModel>()
                .ReverseMap();

            CreateMap<PortionPriceViewModel, PortionPriceModel>()
                .ReverseMap();

            CreateMap<ProductGroupViewModel, ProductGroupModel>()
                .ReverseMap();

            CreateMap<ProductViewModel, ProductModel>()
                .ReverseMap();

            CreateMap<CreateDispatcherViewModel, CreateDispatcherModel>()
                .ReverseMap();

            CreateMap<GetDispatcherViewModel, GetDispatcherModel>()
                .ReverseMap();

            CreateMap<UpdateDispatcherViewModel, UpdateDispatcherModel>()
                .ReverseMap();

            CreateMap<RestaurantPartialViewModel, RestaurantPartialModel>()
                .ReverseMap();
        }
    }
}
