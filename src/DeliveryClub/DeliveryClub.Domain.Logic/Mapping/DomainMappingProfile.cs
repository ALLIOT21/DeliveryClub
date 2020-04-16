using AutoMapper;
using DeliveryClub.Data.DTO.ActorsDTO;
using DeliveryClub.Data.DTO.EntitiesDTO;
using DeliveryClub.Domain.Models.Actors;
using DeliveryClub.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Domain.Logic.Mapping
{
    public class DomainMappingProfile : Profile
    {
        public DomainMappingProfile()
        {
            CreateMap<Admin, AdminDTO>().ReverseMap();
            CreateMap<Restaurant, RestaurantDTO>().ReverseMap();
        }
    }
}
