using AutoMapper;
using DeliveryClub.Domain.AuxiliaryModels.SuperUser;
using DeliveryClub.Web.ViewModels.SuperUser;
using DeliveryClub.Web.ViewModels.Admin;
using DeliveryClub.Domain.AuxiliaryModels.Admin;

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
        }
    }
}
