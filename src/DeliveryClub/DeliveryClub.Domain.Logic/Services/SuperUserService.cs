﻿using DeliveryClub.Data.Context;
using DeliveryClub.Data.DTO.ActorsDTO;
using DeliveryClub.Data.DTO.EntitiesDTO;
using DeliveryClub.Domain.AuxiliaryModels.SuperUser;
using DeliveryClub.Domain.Logic.Interfaces;
using DeliveryClub.Domain.Models;
using DeliveryClub.Domain.Models.Actors;
using DeliveryClub.Domain.Models.Entities;
using DeliveryClub.Infrastructure.Mapping;
using Microsoft.AspNetCore.Identity;
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

        public IEnumerable<GetAdminModel> GetAdmins()
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

            var adminsView = new List<GetAdminModel>();
            foreach (var ad in admins)
            {
                adminsView.Add(new GetAdminModel()
                {
                    Id = ad.Id,
                    RestaurantName = ad.Restaurant.Name,
                    Email = ad.User.Email
                });
            }

            return adminsView;
        }

        public async Task<UpdateAdminModel> GetAdmin(int id)
        {
            var adminDto = await _dbContext.Admins.FindAsync(id);
            adminDto.User = _dbContext.Users.Find(adminDto.UserId);
            adminDto.Restaurant = _dbContext.Restaurants.Find(adminDto.RestaurantId);
            var updateAdminModel = new UpdateAdminModel()
            {
                OldEmail = adminDto.User.Email,
            };
            return updateAdminModel;
        }

        public async Task<IdentityResult> CreateAdminAndRestaurant(CreateAdminModel model)
        {
            var result = await CreateIdentityUser(model);
            if (result.Item2.Succeeded)
            {
                var restaurant = CreateRestaurant();
                await CreateAdmin(result.Item1.Id, restaurant.Result.Id);
                await _dbContext.SaveChangesAsync();
            }
            return result.Item2;
        }
        
        public async Task<IdentityResult> UpdateAdmin(UpdateAdminModel model)
        {
            var iuser = _dbContext.Users.Where(u => u.Email == model.OldEmail).FirstOrDefault();
            var passwordValidationResult = ValidatePassword(_userManager, iuser, model.Password);
            if (passwordValidationResult.Succeeded)
            {
                iuser.Email = model.NewEmail;
                iuser.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(iuser, model.Password);
                var updateResult = await _userManager.UpdateAsync(iuser);
                if (updateResult.Succeeded)
                {
                    await _dbContext.SaveChangesAsync();
                }
                return updateResult;
            }
            return passwordValidationResult;
            
        }

        public async Task DeleteAdminAndRestaurant(int id)
        {
            var admin = _dbContext.Admins.Find(id);
            var restaurant = _dbContext.Restaurants.Find(admin.RestaurantId);

            DeleteAdmin(admin.Id);
            DeleteRestaurant(restaurant.Id);
            DeleteRestaurantInfo(restaurant.Id);         
            DeleteIdentityUser(admin.UserId);

            await _dbContext.SaveChangesAsync();
        }

        private async Task<(IdentityUser, IdentityResult)> CreateIdentityUser(CreateAdminModel model)
        {
            var iuser = new IdentityUser { UserName = model.Email, Email = model.Email };
            var passwordValidationResult = ValidatePassword(_userManager, iuser, model.Password);
            if (passwordValidationResult.Succeeded)
            {
                var createResult = await _userManager.CreateAsync(iuser, model.Password);
                if (createResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(iuser, Role.Admin);
                }
                return (iuser, createResult);
            }
            return (iuser, passwordValidationResult);
        }

        private async Task<Admin> CreateAdmin(string iuserId, int restaurantId)
        {
            var admin = new Admin()
            {
                RestaurantId = restaurantId,
                UserId = iuserId,
            };
            var result = await _dbContext.Admins.AddAsync(_mapper.Map<Admin, AdminDTO>(admin));
            return _mapper.Map<AdminDTO, Admin>(result.Entity);
        }

        private async Task<Restaurant> CreateRestaurant()
        {
            var restaurant = new Restaurant();
            var createRestResult = await _dbContext.Restaurants.AddAsync(_mapper.Map<Restaurant, RestaurantDTO>(restaurant));
            await _dbContext.SaveChangesAsync();
            await CreateRestaurantAdditionalInfo(createRestResult.Entity.Id);
            return _mapper.Map<RestaurantDTO, Restaurant>(_dbContext.Restaurants.Find(createRestResult.Entity.Id));
        }        

        private IdentityResult ValidatePassword(UserManager<IdentityUser> userManager, IdentityUser user, string password)
        {
            var passwordValidator = new PasswordValidator<IdentityUser>();
            var passwordValidationResult = passwordValidator.ValidateAsync(userManager, user, password);
            return passwordValidationResult.Result;
        }        

        private async Task<RestaurantAdditionalInfo> CreateRestaurantAdditionalInfo(int restaurantId)
        {
            var info = new RestaurantAdditionalInfo
            {
                RestaurantId = restaurantId,
            };
            var createResult = await _dbContext.RestaurantAdditionalInfos.AddAsync(_mapper.Map<RestaurantAdditionalInfo, RestaurantAdditionalInfoDTO>(info));
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<RestaurantAdditionalInfoDTO, RestaurantAdditionalInfo>(createResult.Entity);
           
        }

        private void DeleteRestaurant(int id)
        {
            _dbContext.Restaurants.Remove(_dbContext.Restaurants.Find(id));            
        }

        private void DeleteRestaurantInfo(int id)
        {
            _dbContext.RestaurantAdditionalInfos.Remove(_dbContext.RestaurantAdditionalInfos.Where(o => o.RestaurantId == id).FirstOrDefault());
        }

        private void DeleteIdentityUser(string id)
        {
            _dbContext.Users.Remove(_dbContext.Users.Find(id));
        }

        private void DeleteAdmin(int id)
        {
            _dbContext.Admins.Remove(_dbContext.Admins.Find(id));
        }
    }
}