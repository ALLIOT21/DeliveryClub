using DeliveryClub.Data.Context;
using DeliveryClub.Data.DTO.EntitiesDTO;
using DeliveryClub.Domain.AuxiliaryModels.Admin;
using DeliveryClub.Domain.Models.Entities;
using DeliveryClub.Infrastructure.Mapping;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DeliveryClub.Domain.Logic.Managers
{
    public class RestaurantManager
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly Mapper _mapper;
        private readonly RestaurantAdditionalInfoManager _restaurantAdditionalInfoManager;
        private readonly SpecializationManager _specializationManager;

        public RestaurantManager(ApplicationDbContext dbContext,
                            UserManager<IdentityUser> userManager,
                            RestaurantAdditionalInfoManager restaurantAdditionalInfoManager,
                            SpecializationManager specializationManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
            _restaurantAdditionalInfoManager = restaurantAdditionalInfoManager;
            _specializationManager = specializationManager;
        }

        public Restaurant GetRestaurant(int id)
        {
            var restaurantDTO = _dbContext.Restaurants.Where(r => r.Id == id).FirstOrDefault();
            var restaurant = _mapper.Map<RestaurantDTO, Restaurant>(restaurantDTO);
            restaurant.RestaurantAdditionalInfo = _restaurantAdditionalInfoManager.GetRestaurantAdditionalInfo(restaurant.Id);
            var specializations = _specializationManager.GetSpecializations(restaurant);
            restaurant.Specializations = specializations;

            return restaurant;
        }

        public async Task<Restaurant> UpdateRestaurant(Restaurant restaurant, RestaurantInfoModel restaurantInfoModel)
        {
            restaurant.Name = restaurantInfoModel.Name;
            await _specializationManager.UpdateSpecializations(restaurant, _specializationManager.CreateSpecializationHashSet(restaurantInfoModel.Specializations));
            restaurant.DeliveryCost = restaurantInfoModel.DeliveryCost;
            restaurant.MinimalOrderPrice = restaurantInfoModel.MinimalOrderPrice;
            var updateInfoResult = await _restaurantAdditionalInfoManager.UpdateRestaurantAdditionalInfo(restaurant.RestaurantAdditionalInfo, restaurantInfoModel);
            restaurant.RestaurantAdditionalInfo = updateInfoResult;
            var updateResultDTO = _dbContext.Restaurants.Update(_mapper.Map<Restaurant, RestaurantDTO>(restaurant));
            var updateResult = _mapper.Map<RestaurantDTO, Restaurant>(updateResultDTO.Entity);
            updateResult.Specializations = _specializationManager.GetSpecializations(updateResult);
            updateResult.RestaurantAdditionalInfo = _restaurantAdditionalInfoManager.GetRestaurantAdditionalInfo(updateResult.Id);
            await _dbContext.SaveChangesAsync();
            return updateResult;
        }
    }
}
