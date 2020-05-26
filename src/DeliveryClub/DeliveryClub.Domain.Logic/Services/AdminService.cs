using DeliveryClub.Data.Context;
using DeliveryClub.Data.DTO.EntitiesDTO;
using DeliveryClub.Domain.AuxiliaryModels.Admin;
using DeliveryClub.Domain.Logic.Extensions;
using DeliveryClub.Domain.Logic.Interfaces;
using DeliveryClub.Domain.Logic.Managers;
using DeliveryClub.Domain.Models.Entities;
using DeliveryClub.Infrastructure.Mapping;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DeliveryClub.Domain.Logic.Services
{
    public class AdminService : IAdminService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly Mapper _mapper;

        private readonly AdminManager _adminManager;
        private readonly PaymentMethodManager _paymentMethodManager;
        private readonly PortionPriceManager _portionPriceManager;
        private readonly PortionPriceProductGroupManager _portionPriceProductGroupManager;
        private readonly PortionPriceProductManager _portionPriceProductManager;
        private readonly ProductGroupManager _productGroupManager;
        private readonly ProductManager _productManager;
        private readonly RestaurantAdditionalInfoManager _restaurantAdditionalInfoManager;
        private readonly RestaurantManager _restaurantManager;
        private readonly SpecializationManager _specializationManager;


        public AdminService(ApplicationDbContext dbContext,
                            UserManager<IdentityUser> userManager,
                            AdminManager adminManager,
                            PaymentMethodManager paymentMethodManager,
                            PortionPriceManager portionPriceManager,
                            PortionPriceProductGroupManager portionPriceProductGroupManager,
                            PortionPriceProductManager portionPriceProductManager,
                            ProductGroupManager productGroupManager,
                            ProductManager productManager,
                            RestaurantAdditionalInfoManager restaurantAdditionalInfoManager,
                            RestaurantManager restaurantManager,
                            SpecializationManager specializationManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
            _adminManager = adminManager;
            _paymentMethodManager = paymentMethodManager;
            _portionPriceManager = portionPriceManager;
            _portionPriceProductGroupManager = portionPriceProductGroupManager;
            _portionPriceProductManager = portionPriceProductManager;
            _productGroupManager = productGroupManager;
            _productManager = productManager;
            _restaurantAdditionalInfoManager = restaurantAdditionalInfoManager;
            _restaurantManager = restaurantManager;
            _specializationManager = specializationManager;
        }

        public async Task<RestaurantInfoModel> GetRestaurantInfo(ClaimsPrincipal currentUser)
        {
            var user = await _userManager.GetCurrentIdentityUser(currentUser);
            var admin = _adminManager.GetAdmin(user.Id);
            var restaurant = _restaurantManager.GetRestaurant(admin.RestaurantId);
            var restaurantInfo = CreateRestaurantInfoModel(restaurant);
            return restaurantInfo;
        }

        public async Task<RestaurantInfoModel> UpdateRestaurantInfo(ClaimsPrincipal currentUser, RestaurantInfoModel restaurantInfoModel)
        {
            var user = await _userManager.GetCurrentIdentityUser(currentUser);
            var admin = _adminManager.GetAdmin(user.Id);
            var restaurant = _restaurantManager.GetRestaurant(admin.RestaurantId);
            var updatedRestaurant = await _restaurantManager.UpdateRestaurant(restaurant, restaurantInfoModel);
            var result = CreateRestaurantInfoModel(updatedRestaurant);
            return result;
        }

        public async Task<ProductGroupModel> CreateProductGroup(ClaimsPrincipal currentUser, ProductGroupModel model)
        {
            var currentIdentityUser = await _userManager.GetCurrentIdentityUser(currentUser);
            var admin = _adminManager.GetAdmin(currentIdentityUser.Id);
            var restaurant = _restaurantManager.GetRestaurant(admin.RestaurantId);

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

        public async Task<ICollection<ProductGroupModel>> GetProductGroups(ClaimsPrincipal currentUser)
        {
            var currentIdentityUser = await _userManager.GetCurrentIdentityUser(currentUser);
            var admin = _adminManager.GetAdmin(currentIdentityUser.Id);
            var restaurant = _restaurantManager.GetRestaurant(admin.RestaurantId);
            var productGroups = _productGroupManager.GetProductGroupsFull(restaurant);
            var result = CreateProductGroupModels(productGroups);

            return result;
        }

        public async Task DeleteProductGroup(ClaimsPrincipal currentUser, int id)
        {
            var currentIdentityUser = await _userManager.GetCurrentIdentityUser(currentUser);
            var admin = _adminManager.GetAdmin(currentIdentityUser.Id);
            var restaurant = _restaurantManager.GetRestaurant(admin.RestaurantId);

            var productGroupDTO = _productGroupManager.GetProductGroupDTOById(id);
            foreach (var pp in productGroupDTO.PortionPrices)
            {
                var resultPPPG = _dbContext.PortionPriceProductGroups.Remove(pp);
                var resultPP = _dbContext.PortionPrices.Remove(pp.PortionPrice);
            }
            _dbContext.ProductGroups.Remove(productGroupDTO);

            _dbContext.SaveChanges();
        }

        public async Task UpdateProductGroup(ClaimsPrincipal currentUser, ProductGroupModel model)
        {
            var productGroupDTO = _productGroupManager.GetProductGroupDTOById(model.Id);
            var productGroup = _mapper.Map<ProductGroupDTO, ProductGroup>(productGroupDTO);

            productGroup.Name = model.Name;
            var portionPrices = _portionPriceManager.CreatePortionPrices(model.PortionPrices);
            _portionPriceProductGroupManager.UpdatePortionPricesProductGroup(portionPrices, productGroup);
            _dbContext.ProductGroups.Update(_mapper.Map<ProductGroup, ProductGroupDTO>(productGroup));
            await _dbContext.SaveChangesAsync();
        }

        public async Task CreateProduct(ClaimsPrincipal currentUser, ProductModel model)
        {
            var currentIdentityUser = await _userManager.GetCurrentIdentityUser(currentUser);
            var admin = _adminManager.GetAdmin(currentIdentityUser.Id);
            var restaurant = _restaurantManager.GetRestaurant(admin.RestaurantId);            

            var productGroup = _productGroupManager.GetProductGroup(restaurant.Id, model.ProductGroupName);
            var product = _productManager.CreateProduct(model, productGroup.Id);            
        }

        private RestaurantInfoModel CreateRestaurantInfoModel(Restaurant restaurant)
        {
            var restaurantInfo = new RestaurantInfoModel();
            var stringTimeSpanConverter = new StringTimeSpanConverter();

            restaurantInfo.Name = restaurant.Name;
            restaurantInfo.Specializations = _specializationManager.CreateSpecializationList(restaurant.Specializations);
            restaurantInfo.DeliveryCost = restaurant.DeliveryCost;
            restaurantInfo.MinimalOrderPrice = restaurant.MinimalOrderPrice;
            restaurantInfo.Description = restaurant.RestaurantAdditionalInfo.Description;
            restaurantInfo.PaymentMethods = _paymentMethodManager.CreatePaymentMethodList(restaurant.RestaurantAdditionalInfo.PaymentMethods);
            restaurantInfo.DeliveryMaxTime = stringTimeSpanConverter.TimeSpanToString(restaurant.RestaurantAdditionalInfo.DeliveryMaxTime);
            restaurantInfo.OrderTimeBegin = stringTimeSpanConverter.TimeSpanToString(restaurant.RestaurantAdditionalInfo.OrderTimeBegin);
            restaurantInfo.OrderTimeEnd = stringTimeSpanConverter.TimeSpanToString(restaurant.RestaurantAdditionalInfo.OrderTimeEnd);
            return restaurantInfo;
        }        

        private ICollection<ProductGroupModel> CreateProductGroupModels(ICollection<ProductGroup> productGroups)
        {
            var result = new List<ProductGroupModel>();
            foreach (var pg in productGroups)
            {
                var productGroupModel = new ProductGroupModel
                {
                    Id = pg.Id,
                    Name = pg.Name,
                    PortionPrices = CreatePortionPriceModels(pg.PortionPrices).ToList(),
                    Products = CreateProductModels(pg.Products).ToList()
                };
                result.Add(productGroupModel);
            }
            return result;
        }        

        private ICollection<PortionPriceModel> CreatePortionPriceModels(ICollection<PortionPriceProductGroup> portionPrices)
        {
            var result = new List<PortionPriceModel>();
            foreach (var pp in portionPrices)
            {
                var ppm = new PortionPriceModel
                {
                    Id = pp.PortionPrice.Id,
                    Portion = pp.PortionPrice.Portion,
                    Price = pp.PortionPrice.Price,
                };
                result.Add(ppm);
            }
            return result;
        }

        private ICollection<PortionPriceModel> CreatePortionPriceModels(ICollection<PortionPriceProduct> portionPrices)
        {
            var result = new List<PortionPriceModel>();
            foreach (var pp in portionPrices)
            {
                var ppm = new PortionPriceModel
                {
                    Id = pp.PortionPrice.Id,
                    Portion = pp.PortionPrice.Portion,
                    Price = pp.PortionPrice.Price,
                };
                result.Add(ppm);
            }
            return result;
        }

        private ICollection<ProductModel> CreateProductModels(ICollection<Product> products)
        {
            var result = new List<ProductModel>();
            foreach (var p in products)
            {
                var productModel = new ProductModel
                {
                    Name = p.Name,
                    Description = p.Description,
                    Id = p.Id,
                    PortionPrices = CreatePortionPriceModels(p.PortionPrices).ToList()
                };
                result.Add(productModel);
            }
            return result;
        }
    }
}
