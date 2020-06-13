using DeliveryClub.Data.Context;
using DeliveryClub.Domain.AuxiliaryModels.Admin;
using DeliveryClub.Domain.Logic.Extensions;
using DeliveryClub.Domain.Logic.Interfaces;
using DeliveryClub.Domain.Logic.Managers;
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
        private readonly Mapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AdminManager _adminManager;
        private readonly PaymentMethodManager _paymentMethodManager;
        private readonly PortionPriceManager _portionPriceManager;
        private readonly PortionPriceProductGroupManager _portionPriceProductGroupManager;
        private readonly ProductGroupManager _productGroupManager;
        private readonly ProductManager _productManager;
        private readonly RestaurantManager _restaurantManager;
        private readonly SpecializationManager _specializationManager;

        private readonly DispatcherManager _dispatcherManager;
        private readonly IdentityUserManager _identityUserManager;

        public AdminService(ApplicationDbContext dbContext,
                            UserManager<IdentityUser> userManager,
                            AdminManager adminManager,
                            DispatcherManager dispatcherManager,
                            IdentityUserManager identityUserManager,
                            PaymentMethodManager paymentMethodManager,
                            PortionPriceManager portionPriceManager,
                            PortionPriceProductGroupManager portionPriceProductGroupManager,
                            PortionPriceProductManager portionPriceProductManager,
                            ProductGroupManager productGroupManager,
                            ProductManager productManager,
                            RestaurantManager restaurantManager,
                            SpecializationManager specializationManager,
                            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
            _adminManager = adminManager;
            _dispatcherManager = dispatcherManager;
            _identityUserManager = identityUserManager;
            _paymentMethodManager = paymentMethodManager;
            _portionPriceManager = portionPriceManager;
            _portionPriceProductGroupManager = portionPriceProductGroupManager;
            _productGroupManager = productGroupManager;
            _productManager = productManager;
            _restaurantManager = restaurantManager;
            _specializationManager = specializationManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<RestaurantInfoModel> GetRestaurantInfo()
        {
            var restaurant = await GetCurrentRestaurant();
            var restaurantInfo = CreateRestaurantInfoModel(restaurant);
            return restaurantInfo;
        }

        public async Task<RestaurantInfoModel> UpdateRestaurantInfo(RestaurantInfoModel restaurantInfoModel)
        {
            var restaurant = await GetCurrentRestaurant();
            var updatedRestaurant = await _restaurantManager.UpdateRestaurant(restaurant, restaurantInfoModel);
            var result = CreateRestaurantInfoModel(updatedRestaurant);
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
            var result = CreateProductGroupModels(productGroups);

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

            return CreateProductModel(product);
        }

        public async Task<ProductModel> UpdateProduct(ProductModel model)
        {
            var currentIdentityUser = await _userManager.GetCurrentIdentityUser(_httpContextAccessor.HttpContext.User);
            var admin = _adminManager.GetAdmin(currentIdentityUser.Id);
            var restaurant = _restaurantManager.GetRestaurant(admin.RestaurantId);

            var productGroup = _productGroupManager.GetProductGroup(restaurant.Id, model.ProductGroupName);
            var updatedProduct = await _productManager.UpdateProduct(model, productGroup.Id);

            return CreateProductModel(updatedProduct);
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

            return CreateGetDispatcherModels(dispatchers);
        }

        public UpdateDispatcherModel GetDispatcher(int id)
        {
            var d = _dispatcherManager.GetDispatcher(id);            

            return CreateUpdateDispatcherModel(d);
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

        private ProductModel CreateProductModel(Product product)
        {
            var productModel = new ProductModel
            {
                Name = product.Name,
                Description = product.Description,
                Id = product.Id,
                ImageName = product.ImageName,
                ProductGroupName = product.ProductGroup.Name,
                PortionPrices = CreatePortionPriceModels(product.PortionPrices).ToList()
            };
            return productModel;
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
                    ImageName = p.ImageName,
                    PortionPrices = CreatePortionPriceModels(p.PortionPrices).ToList()
                };
                result.Add(productModel);
            }
            return result;
        }

        private ICollection<GetDispatcherModel> CreateGetDispatcherModels(ICollection<Dispatcher> dispatchers)
        {
            var getDispatcherModels = new List<GetDispatcherModel>();
            foreach (var d in dispatchers)
            {
                var gdm = new GetDispatcherModel()
                {
                    Email = d.User.Email,
                    Id = d.Id,
                };
                getDispatcherModels.Add(gdm);
            }
            return getDispatcherModels;
        }
        
        private UpdateDispatcherModel CreateUpdateDispatcherModel(Dispatcher dispatcher)
        {
            var udm = new UpdateDispatcherModel
            {
                Id = dispatcher.Id,
                OldEmail = dispatcher.User.Email,
            };
            return udm;
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
