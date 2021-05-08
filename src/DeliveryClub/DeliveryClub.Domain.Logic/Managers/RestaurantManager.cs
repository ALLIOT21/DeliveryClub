using DeliveryClub.Data.Context;
using DeliveryClub.Data.DTO.EntitiesDTO;
using DeliveryClub.Domain.AuxiliaryModels.Admin;
using DeliveryClub.Domain.Logic.Mapping;
using DeliveryClub.Domain.Models.Entities;
using DeliveryClub.Infrastructure.Mapping;
using DeliveryClub.Infrastructure.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DeliveryClub.Domain.Logic.Managers
{
    public class RestaurantManager
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Mapper _mapper;
        private readonly RestaurantAdditionalInfoManager _restaurantAdditionalInfoManager;
        private readonly SpecializationManager _specializationManager;
        private readonly AuxiliaryMapper _auxiliaryMapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ProductGroupManager _productGroupManager;

        public RestaurantManager(ApplicationDbContext dbContext,
                            RestaurantAdditionalInfoManager restaurantAdditionalInfoManager,
                            SpecializationManager specializationManager,
                            AuxiliaryMapper auxiliaryMapper,
                            IWebHostEnvironment webHostEnvironment,
                            ProductGroupManager productGroupManager)
        {
            _dbContext = dbContext;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
            _restaurantAdditionalInfoManager = restaurantAdditionalInfoManager;
            _specializationManager = specializationManager;
            _auxiliaryMapper = auxiliaryMapper;
            _webHostEnvironment = webHostEnvironment;
            _productGroupManager = productGroupManager;
        }

        public Restaurant GetRestaurant(int id)
        {
            var restaurantDTO = _dbContext.Restaurants.Where(r => r.Id == id).FirstOrDefault();
            var restaurant = _mapper.Map<RestaurantDAO, Restaurant>(restaurantDTO);
            restaurant.RestaurantAdditionalInfo = _restaurantAdditionalInfoManager.GetRestaurantAdditionalInfo(restaurant.Id);
            var specializations = _specializationManager.GetSpecializations(restaurant);
            restaurant.Specializations = specializations;

            return restaurant;
        }

        public Restaurant GetRestaurantFull(int id)
        {
            var restaurantDTO = _dbContext.Restaurants.Where(r => r.Id == id).FirstOrDefault();
            var restaurant = _mapper.Map<RestaurantDAO, Restaurant>(restaurantDTO);
            restaurant.RestaurantAdditionalInfo = _restaurantAdditionalInfoManager.GetRestaurantAdditionalInfo(restaurant.Id);
            var specializations = _specializationManager.GetSpecializations(restaurant);
            restaurant.Specializations = specializations;

            restaurant.Menu = _productGroupManager.GetProductGroupsFull(restaurant);

            return restaurant;
        }

        public ICollection<Restaurant> GetRestaurantsPartially()
        {
            var result = new List<Restaurant>();
            foreach(var rdto in _dbContext.Restaurants.ToList())
            {
                var r = _mapper.Map<RestaurantDAO, Restaurant>(rdto);

                r.Specializations = _specializationManager.GetSpecializations(r);

                result.Add(r);
            }
            return result;
        }

        public async Task<Restaurant> UpdateRestaurant(Restaurant restaurant, RestaurantInfoModel restaurantInfoModel)
        {
            restaurant.Name = restaurantInfoModel.Name;
            await _specializationManager.UpdateSpecializations(restaurant, _auxiliaryMapper.CreateSpecializationHashSet(restaurantInfoModel.Specializations));
            restaurant.DeliveryCost = restaurantInfoModel.DeliveryCost;
            restaurant.MinimalOrderPrice = restaurantInfoModel.MinimalOrderPrice;
                        
            if (restaurantInfoModel.CoverImage != null)
            {
                string fileName;
                var imgServ = new ImageService(_webHostEnvironment.WebRootPath);
                if (restaurant.CoverImageName != null)
                {
                    imgServ.DeleteImage(restaurant.CoverImageName);
                    fileName = await imgServ.CreateImage(restaurantInfoModel.CoverImage);                    
                }
                else
                {
                    fileName = await imgServ.CreateImage(restaurantInfoModel.CoverImage);
                }
                restaurant.CoverImageName = fileName;
            }

            var updateInfoResult = await _restaurantAdditionalInfoManager.UpdateRestaurantAdditionalInfo(restaurant.RestaurantAdditionalInfo, restaurantInfoModel);
            restaurant.RestaurantAdditionalInfo = updateInfoResult;
            var updateResultDTO = _dbContext.Restaurants.Update(_mapper.Map<Restaurant, RestaurantDAO>(restaurant));
            var updateResult = _mapper.Map<RestaurantDAO, Restaurant>(updateResultDTO.Entity);
            updateResult.Specializations = _specializationManager.GetSpecializations(updateResult);
            updateResult.RestaurantAdditionalInfo = _restaurantAdditionalInfoManager.GetRestaurantAdditionalInfo(updateResult.Id);
            await _dbContext.SaveChangesAsync();
            return updateResult;
        }
    }
}
