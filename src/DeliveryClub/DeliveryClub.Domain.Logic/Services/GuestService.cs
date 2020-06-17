using DeliveryClub.Data.Context;
using DeliveryClub.Domain.AuxiliaryModels.Guest;
using DeliveryClub.Domain.Logic.Interfaces;
using DeliveryClub.Domain.Logic.Managers;
using DeliveryClub.Domain.Logic.Mapping;
using DeliveryClub.Domain.Models;
using DeliveryClub.Domain.Models.Entities;
using DeliveryClub.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace DeliveryClub.Domain.Logic.Services
{
    public class GuestService : IGuestService
    {
        private readonly RestaurantManager _restaurantManager;
        private readonly AuxiliaryMapper _auxiliaryMapper;
        private readonly SignInManager<IdentityUser> _signInManager;

        public GuestService(SignInManager<IdentityUser> signInManager,
                            RestaurantManager restaurantManager,
                            AuxiliaryMapper auxiliaryMapper)
        {
            _signInManager = signInManager;
            _restaurantManager = restaurantManager;
            _auxiliaryMapper = auxiliaryMapper;
        }

        public ICollection<RestaurantPartialModel> GetRestaurantsPartially()
        {
            var restaurants = _restaurantManager.GetRestaurantsPartially();

            var rpartials = CreateRestaurantPartialModels(restaurants);
            return rpartials;
        }

        private ICollection<RestaurantPartialModel> CreateRestaurantPartialModels(ICollection<Restaurant> restaurants)
        {
            var rpms = new List<RestaurantPartialModel>();
            
            foreach (var r in restaurants)
            {
                var rpm = new RestaurantPartialModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    DeliveryCost = r.DeliveryCost,
                    MinimalOrderPrice = r.MinimalOrderPrice,
                    CoverImageName = r.CoverImageName,
                    Specializations = _auxiliaryMapper.CreateSpecializationModelList(r.Specializations)
                };
                rpms.Add(rpm);
            }
            return rpms;
        }

        public string GetUserRole()
        {
            var userClaims = GetUserClaims();
            if (userClaims.IsUser())
            {
                return Role.User;
            }
            else
            if (userClaims.IsSuperUser())
            {
                return Role.SuperUser;
            }
            else
            if (userClaims.IsAdmin())
            {
                return Role.Admin;
            }
            else if (userClaims.IsDispatcher())
            {
                return Role.Dispatcher;
            }
            else
            {
                return null;
            }
        }

        private ClaimsPrincipal GetUserClaims()
        {
            return _signInManager.Context.User;
        }
    }
}
