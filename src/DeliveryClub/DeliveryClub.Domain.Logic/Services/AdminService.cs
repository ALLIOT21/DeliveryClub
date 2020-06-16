using DeliveryClub.Data.Context;
using DeliveryClub.Domain.AuxiliaryModels.Admin;
using DeliveryClub.Domain.Logic.Extensions;
using DeliveryClub.Domain.Logic.Interfaces;
using DeliveryClub.Domain.Logic.Managers;
using DeliveryClub.Domain.Logic.Mapping;
using DeliveryClub.Domain.Models.Actors;
using DeliveryClub.Domain.Models.Entities;
using DeliveryClub.Infrastructure.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DeliveryClub.Domain.Logic.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AdminManager _adminManager;
        private readonly PortionPriceProductGroupManager _portionPriceProductGroupManager;
        private readonly AuxiliaryMapper _auxiliaryMapper;
        private readonly ProductGroupManager _productGroupManager;
        private readonly ProductManager _productManager;
        private readonly RestaurantManager _restaurantManager;
        private readonly DispatcherManager _dispatcherManager;
        private readonly IdentityUserManager _identityUserManager;

        public AdminService(ApplicationDbContext dbContext,
                            UserManager<IdentityUser> userManager,
                            AdminManager adminManager,
                            DispatcherManager dispatcherManager,
                            IdentityUserManager identityUserManager,
                            PortionPriceProductGroupManager portionPriceProductGroupManager,
                            AuxiliaryMapper auxiliaryMapper,
                            ProductGroupManager productGroupManager,
                            ProductManager productManager,
                            RestaurantManager restaurantManager,
                            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _adminManager = adminManager;
            _dispatcherManager = dispatcherManager;
            _identityUserManager = identityUserManager;
            _portionPriceProductGroupManager = portionPriceProductGroupManager;
            _auxiliaryMapper = auxiliaryMapper;
            _productGroupManager = productGroupManager;
            _productManager = productManager;
            _restaurantManager = restaurantManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<RestaurantInfoModel> GetRestaurantInfo()
        {
            var restaurant = await GetCurrentRestaurant();
            var restaurantInfo = _auxiliaryMapper.CreateRestaurantInfoModel(restaurant);
            return restaurantInfo;
        }

        public async Task<RestaurantInfoModel> UpdateRestaurantInfo(RestaurantInfoModel restaurantInfoModel)
        {
            var restaurant = await GetCurrentRestaurant();
            var updatedRestaurant = await _restaurantManager.UpdateRestaurant(restaurant, restaurantInfoModel);
            var result = _auxiliaryMapper.CreateRestaurantInfoModel(updatedRestaurant);
            return result;
        }

        public async Task<ProductGroupModel> CreateProductGroup(ProductGroupModel model)
        {
            var restaurant = await GetCurrentRestaurant();

            var newProductGroup = _productGroupManager.CreateProductGroup(model, restaurant);

            var newPortionPriceModels = new List<PortionPriceModel>();
            foreach (var pp in newProductGroup.PortionPrices)
            {
                var newPortionPriceModel = new PortionPriceModel
                {
                    Id = pp.PortionPriceId,
                    Portion = pp.PortionPrice.Portion,
                    Price = pp.PortionPrice.Price,
                };
                newPortionPriceModels.Add(newPortionPriceModel);
            }

            var newProductGroupModel = new ProductGroupModel
            {
                Id = newProductGroup.Id,
                Name = newProductGroup.Name,
                PortionPrices = newPortionPriceModels,
            };

            await _dbContext.SaveChangesAsync();

            return newProductGroupModel;
        }

        public async Task<ICollection<ProductGroupModel>> GetProductGroups()
        {
            var restaurant = await GetCurrentRestaurant();
            var productGroups = _productGroupManager.GetProductGroupsFull(restaurant);
            var result = _auxiliaryMapper.CreateProductGroupModels(productGroups);

            return result;
        }

        public async Task DeleteProductGroup(int id)
        {
            var productGroupDTO = _productGroupManager.GetProductGroupDTOById(id);
            foreach (var pp in productGroupDTO.PortionPrices)
            {
                _dbContext.PortionPriceProductGroups.Remove(pp);
                _dbContext.PortionPrices.Remove(pp.PortionPrice);
            }

            _dbContext.ProductGroups.Remove(productGroupDTO);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateProductGroup(ProductGroupModel model)
        {
            await _productGroupManager.UpdateProductGroup(model);
        }

        public async Task CreateProduct(ProductModel model)
        {
            var restaurantId = await GetCurrentRestaurantId();

            var productGroup = _productGroupManager.GetProductGroup(restaurantId, model.ProductGroupName);
            await _productManager.CreateProduct(model, productGroup.Id);
        }

        public ProductModel GetProduct(int id)
        {
            var product = _productManager.GetProduct(id);

            return _auxiliaryMapper.CreateProductModel(product);
        }

        public async Task<ProductModel> UpdateProduct(ProductModel model)
        {
            var currentIdentityUser = await _userManager.GetCurrentIdentityUser(_httpContextAccessor.HttpContext.User);
            var admin = _adminManager.GetAdmin(currentIdentityUser.Id);
            var restaurant = _restaurantManager.GetRestaurant(admin.RestaurantId);

            var productGroup = _productGroupManager.GetProductGroup(restaurant.Id, model.ProductGroupName);
            var updatedProduct = await _productManager.UpdateProduct(model, productGroup.Id);

            return _auxiliaryMapper.CreateProductModel(updatedProduct);
        }

        public async Task<IdentityResult> CreateDispatcher(CreateDispatcherModel model)
        {
            var restaurantId = await GetCurrentRestaurantId();

            var result = await _identityUserManager.CreateIdentityUser(model.Email, model.Password);
            if (result.Item2.Succeeded)
            {
                await _dispatcherManager.CreateDispatcher(result.Item1.Id, restaurantId);
                await _dbContext.SaveChangesAsync();
            }
            return result.Item2;
        }

        public async Task<ICollection<GetDispatcherModel>> GetDispatchers()
        {
            var restaurantId = await GetCurrentRestaurantId();

            var dispatchers = _dispatcherManager.GetDispatchers(restaurantId);

            return _auxiliaryMapper.CreateGetDispatcherModels(dispatchers);
        }

        public UpdateDispatcherModel GetDispatcher(int id)
        {
            var d = _dispatcherManager.GetDispatcher(id);            

            return _auxiliaryMapper.CreateUpdateDispatcherModel(d);
        }

        public async Task<IdentityResult> UpdateDispatcher(UpdateDispatcherModel model)
        {
            var result = await _dispatcherManager.UpdateDispatcher(model);
            return result;
        }

        public async Task DeleteDispatcher(int id)
        {
            await _dispatcherManager.DeleteDispatcher(id);
        }
        
        public async Task DeleteProduct(int id)
        {
            await _productManager.DeleteProduct(id);
        }

        public async Task<bool> HasPortionPrices(string productGroupName)
        {
            var restaurantId = await GetCurrentRestaurantId();

            var productGroup = _productGroupManager.GetProductGroup(restaurantId, productGroupName);
            var portionPricesProductGroup = _portionPriceProductGroupManager.GetPortionPriceProductGroups(productGroup.Id);

            if (portionPricesProductGroup.Count > 0)
                return true;
            else
                return false;
        }      

        private async Task<int> GetCurrentRestaurantId()
        {
            var currentIdentityUser = await _userManager.GetCurrentIdentityUser(_httpContextAccessor.HttpContext.User);
            var admin = _adminManager.GetAdmin(currentIdentityUser.Id);
            return admin.RestaurantId;
        }

        private async Task<Restaurant> GetCurrentRestaurant()
        {
            var currentIdentityUser = await _userManager.GetCurrentIdentityUser(_httpContextAccessor.HttpContext.User);
            var admin = _adminManager.GetAdmin(currentIdentityUser.Id);
            return _restaurantManager.GetRestaurant(admin.RestaurantId);
        }
    }
}
