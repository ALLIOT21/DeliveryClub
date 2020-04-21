using AutoMapper;
using DeliveryClub.Domain.AuxiliaryModels;
using DeliveryClub.Web.ViewModels;

namespace DeliveryClub.Web.Mapping
{
    public class ViewMappingProfile : Profile
    {
        public ViewMappingProfile()
        {
            CreateMap<CreateAdminViewModel, CreateAdminModel>().ReverseMap();
            CreateMap<GetAdminViewModel, GetAdminModel>().ReverseMap();
            CreateMap<UpdateAdminViewModel, UpdateAdminModel>().ReverseMap();
        }
    }
}
