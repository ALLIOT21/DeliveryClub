using DeliveryClub.Data.Context;
using DeliveryClub.Data.DTO.ActorsDTO;
using DeliveryClub.Data.DTO.EntitiesDTO;
using DeliveryClub.Domain.AuxiliaryModels;
using DeliveryClub.Domain.Logic.Interfaces;
using DeliveryClub.Domain.Models;
using DeliveryClub.Domain.Models.Actors;
using DeliveryClub.Domain.Models.Entities;
using DeliveryClub.Infrastructure.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DeliveryClub.Domain.Logic.Services
{
    public class SuperUserService : ISuperUserService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly Mapper _mapper;

        public SuperUserService(ApplicationDbContext dbContext,
                                UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
        }

        public async Task<IdentityResult> CreateAdminAndRestaurant(CreateAdminModel model)
        {
            var result = await CreateIdentityUser(model);
            if (result.Item2.Succeeded)
            {
                var restaurant = await CreateRestaurant();
                await CreateAdmin(result.Item1, restaurant);
                await _dbContext.SaveChangesAsync();
            }
            return result.Item2;
        }

        public IEnumerable<Admin> GetAdmins()
        {
            var adminsDto = _dbContext.Admins.ToList();
            foreach (var adminDto in adminsDto)
            {
                adminDto.User = _dbContext.Users.Find(adminDto.UserId);
                adminDto.Restaurant = _dbContext.Restaurants.Find(adminDto.RestaurantId);
            }
            var admins = new List<Admin>();
            foreach (var ad in adminsDto)
            {
                admins.Add(_mapper.Map<AdminDTO, Admin>(ad));
            }
            return admins;
        }

        private async Task<(IdentityUser, IdentityResult)> CreateIdentityUser(CreateAdminModel model)
        {
            var iuser = new IdentityUser { UserName = model.Email, Email = model.Email };
            var createResult = await _userManager.CreateAsync(iuser, model.Password);
            if (createResult.Succeeded)
            {
                await _userManager.AddToRoleAsync(iuser, Role.Admin);                
            }
            return (iuser, createResult);
        }

        private async Task<Admin> CreateAdmin(IdentityUser iuser, Restaurant restaurant)
        {
            var admin = new Admin()
            {
                Restaurant = restaurant,
                RestaurantId = restaurant.Id,
                User = iuser,
                UserId = iuser.Id,
            };
            var result = await _dbContext.Admins.AddAsync(_mapper.Map<Admin, AdminDTO>(admin));
            return _mapper.Map<AdminDTO, Admin>(result.Entity);
        }

        private async Task<Restaurant> CreateRestaurant()
        {
            var restaurant = new Restaurant();
            var result = await _dbContext.Restaurants.AddAsync(_mapper.Map<Restaurant, RestaurantDTO>(restaurant));
            return _mapper.Map<RestaurantDTO, Restaurant>(result.Entity);
        }        
    }
}
