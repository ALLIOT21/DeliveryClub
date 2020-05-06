using DeliveryClub.Data.Context;
using DeliveryClub.Data.DTO.ActorsDTO;
using DeliveryClub.Data.DTO.EntitiesDTO;
using DeliveryClub.Domain.AuxiliaryModels.Admin;
using DeliveryClub.Domain.Logic.Interfaces;
using DeliveryClub.Domain.Models.Actors;
using DeliveryClub.Domain.Models.Entities;
using DeliveryClub.Domain.Models.Enumerations;
using DeliveryClub.Infrastructure.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
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

        public AdminService(ApplicationDbContext dbContext,
                            UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
        }

        public async Task<RestaurantInfoModel> GetRestaurantInfo(ClaimsPrincipal currentUser)
        {
            var user = await GetCurrentIdentityUser(currentUser);
            var admin = GetAdmin(user);
            var restaurant = GetRestaurant(admin);
            var restaurantInfo = CreateRestaurantInfoModel(restaurant);
            return restaurantInfo;
        }

        public async Task<RestaurantInfoModel> UpdateRestaurantInfo(ClaimsPrincipal currentUser, RestaurantInfoModel restaurantInfoModel)
        {
            var user = await GetCurrentIdentityUser(currentUser);
            var admin = GetAdmin(user);
            var restaurant = GetRestaurant(admin);
            var updatedRestaurant = await UpdateRestaurant(restaurant, restaurantInfoModel);
            var result = CreateRestaurantInfoModel(updatedRestaurant);
            return result;
        }

        public async Task<ProductGroupModel> CreateProductGroup(ClaimsPrincipal currentUser, ProductGroupModel model)
        {
            var currentIdentityUser = await GetCurrentIdentityUser(currentUser);
            var admin = GetAdmin(currentIdentityUser);
            var restaurant = GetRestaurant(admin);
            var newProductGroup = CreateProductGroup(model, restaurant);

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
            var currentIdentityUser = await GetCurrentIdentityUser(currentUser);
            var admin = GetAdmin(currentIdentityUser);
            var restaurant = GetRestaurant(admin);
            var productGroups = GetProductGroupsFull(restaurant);
            var result = CreateProductGroupModels(productGroups);

            return result;
        }

        public async Task DeleteProductGroup(ClaimsPrincipal currentUser, int id)
        {
            var currentIdentityUser = await GetCurrentIdentityUser(currentUser);
            var admin = GetAdmin(currentIdentityUser);
            var restaurant = GetRestaurant(admin);

            var productGroupDTO = GetProductGroupDTOById(id);
            foreach (var pp in productGroupDTO.PortionPrices)
            {
                var resultPPPG = _dbContext.PortionPriceProductGroups.Remove(pp);
                var resultPP = _dbContext.PortionPrices.Remove(pp.PortionPrice);
            }
            _dbContext.ProductGroups.Remove(productGroupDTO);

            _dbContext.SaveChanges();
        }

        private Restaurant GetRestaurant(Admin admin)
        {
            var restaurantDTO = _dbContext.Restaurants.Where(r => r.Id == admin.RestaurantId).FirstOrDefault();
            var restaurant = _mapper.Map<RestaurantDTO, Restaurant>(restaurantDTO);
            restaurant.RestaurantAdditionalInfo = GetRestaurantAdditionalInfo(restaurant);
            var specializations = GetSpecializations(restaurant);
            restaurant.Specializations = specializations;

            return restaurant;
        }

        private Admin GetAdmin(IdentityUser user)
        {
            var admin = _dbContext.Admins.Where(a => a.UserId == user.Id).FirstOrDefault();
            return _mapper.Map<AdminDTO, Admin>(admin);
        }

        private async Task<IdentityUser> GetCurrentIdentityUser(ClaimsPrincipal currentUser)
        {
            var getUserResult = await _userManager.GetUserAsync(currentUser);
            return getUserResult;
        }

        private RestaurantAdditionalInfo GetRestaurantAdditionalInfo(Restaurant restaurant)
        {
            var restaurantAdditionalInfoDTO = _dbContext.RestaurantAdditionalInfos.Where(rai => rai.RestaurantId == restaurant.Id).FirstOrDefault();
            var restaurantAdditionalInfo = _mapper.Map<RestaurantAdditionalInfoDTO, RestaurantAdditionalInfo>(restaurantAdditionalInfoDTO);
            restaurantAdditionalInfo.PaymentMethods = GetPaymentMethods(restaurantAdditionalInfo);

            return restaurantAdditionalInfo;
        }

        private HashSet<Specialization> GetSpecializations(Restaurant restaurant)
        {
            var specializations = _dbContext.Specializations.Where(s => s.RestaurantId == restaurant.Id).ToHashSet();
            var result = new HashSet<Specialization>();
            foreach (var s in specializations)
            {
                result.Add(s.Specialization);
            }
            return result;
        }

        private HashSet<PaymentMethod> GetPaymentMethods(RestaurantAdditionalInfo restaurantAdditionalInfo)
        {
            var paymentMethods = _dbContext.PaymentMethods.Where(pm => pm.RestaurantAdditionalInfoId == restaurantAdditionalInfo.Id).ToHashSet();
            var result = new HashSet<PaymentMethod>();
            foreach (var pm in paymentMethods)
            {
                result.Add(pm.PaymentMethod);
            }
            return result;
        }

        private async Task UpdateSpecializations(Restaurant restaurant, HashSet<Specialization> newSpecializations)
        {
            var oldSpecializations = GetSpecializations(restaurant);

            foreach (var os in oldSpecializations)
            {
                DeleteSpecialization(restaurant, os);
            }

            foreach (var ns in newSpecializations)
            {
                await CreateSpecialization(restaurant, ns);
            }
        }

        private async Task UpdatePaymentMethods(RestaurantAdditionalInfo restaurantAdditionalInfo, HashSet<PaymentMethod> newPaymentMethods)
        {
            var oldPaymentMethod = GetPaymentMethods(restaurantAdditionalInfo);

            foreach (var osp in oldPaymentMethod)
            {
                DeletePaymentMethod(restaurantAdditionalInfo, osp);
            }

            foreach (var nsp in newPaymentMethods)
            {
                await CreatePaymentMethod(restaurantAdditionalInfo, nsp);
            }
        }

        private async Task<Restaurant> UpdateRestaurant(Restaurant restaurant, RestaurantInfoModel restaurantInfoModel)
        {
            restaurant.Name = restaurantInfoModel.Name;
            await UpdateSpecializations(restaurant, CreateSpecializationHashSet(restaurantInfoModel.Specializations));
            restaurant.DeliveryCost = restaurantInfoModel.DeliveryCost;
            restaurant.MinimalOrderPrice = restaurantInfoModel.MinimalOrderPrice;
            var updateInfoResult = await UpdateRestaurantAdditionalInfo(restaurant.RestaurantAdditionalInfo, restaurantInfoModel);
            restaurant.RestaurantAdditionalInfo = updateInfoResult;
            var updateResultDTO = _dbContext.Restaurants.Update(_mapper.Map<Restaurant, RestaurantDTO>(restaurant));
            var updateResult = _mapper.Map<RestaurantDTO, Restaurant>(updateResultDTO.Entity);
            updateResult.Specializations = GetSpecializations(updateResult);
            updateResult.RestaurantAdditionalInfo = GetRestaurantAdditionalInfo(updateResult);
            await _dbContext.SaveChangesAsync();
            return updateResult;
        }

        private async Task<RestaurantAdditionalInfo> UpdateRestaurantAdditionalInfo(RestaurantAdditionalInfo restaurantAdditionalInfo, RestaurantInfoModel restaurantInfoModel)
        {
            restaurantAdditionalInfo.Description = restaurantInfoModel.Description;
            restaurantAdditionalInfo.DeliveryMaxTime = StringToTimeSpan(restaurantInfoModel.DeliveryMaxTime);
            restaurantAdditionalInfo.OrderTimeBegin = StringToTimeSpan(restaurantInfoModel.OrderTimeBegin);
            restaurantAdditionalInfo.OrderTimeEnd = StringToTimeSpan(restaurantInfoModel.OrderTimeEnd);
            var updateResult = _dbContext.RestaurantAdditionalInfos.Update(_mapper.Map<RestaurantAdditionalInfo, RestaurantAdditionalInfoDTO>(restaurantAdditionalInfo));
            await UpdatePaymentMethods(restaurantAdditionalInfo, CreatePaymentMethodHashSet(restaurantInfoModel.PaymentMethods));
            return _mapper.Map<RestaurantAdditionalInfoDTO, RestaurantAdditionalInfo>(updateResult.Entity);
        }

        private RestaurantInfoModel CreateRestaurantInfoModel(Restaurant restaurant)
        {
            var restaurantInfo = new RestaurantInfoModel();
            restaurantInfo.Name = restaurant.Name;
            restaurantInfo.Specializations = CreateSpecializationList(restaurant.Specializations);
            restaurantInfo.DeliveryCost = restaurant.DeliveryCost;
            restaurantInfo.MinimalOrderPrice = restaurant.MinimalOrderPrice;
            restaurantInfo.Description = restaurant.RestaurantAdditionalInfo.Description;
            restaurantInfo.PaymentMethods = CreatePaymentMethodList(restaurant.RestaurantAdditionalInfo.PaymentMethods);
            restaurantInfo.DeliveryMaxTime = TimeSpanToString(restaurant.RestaurantAdditionalInfo.DeliveryMaxTime);
            restaurantInfo.OrderTimeBegin = TimeSpanToString(restaurant.RestaurantAdditionalInfo.OrderTimeBegin);
            restaurantInfo.OrderTimeEnd = TimeSpanToString(restaurant.RestaurantAdditionalInfo.OrderTimeEnd);
            return restaurantInfo;
        }

        private List<SpecializationModel> CreateSpecializationList(HashSet<Specialization> specializations)
        {
            var result = new List<SpecializationModel>();
            if (specializations != null)
            {
                foreach (var sp in Enum.GetValues(typeof(Specialization)))
                {
                    if (specializations.Contains((Specialization)sp))
                    {
                        result.Add(new SpecializationModel { Specialization = (Specialization)sp, IsSelected = true });
                    }
                    else
                    {
                        result.Add(new SpecializationModel { Specialization = (Specialization)sp, IsSelected = false });
                    }
                }
            }
            return result;
        }

        private List<PaymentMethodModel> CreatePaymentMethodList(HashSet<PaymentMethod> paymentMethods)
        {
            var result = new List<PaymentMethodModel>();
            foreach (var pm in Enum.GetValues(typeof(PaymentMethod)))
            {
                if (paymentMethods.Contains((PaymentMethod)pm))
                {
                    result.Add(new PaymentMethodModel { PaymentMethod = (PaymentMethod)pm, IsSelected = true });
                }
                else
                {
                    result.Add(new PaymentMethodModel { PaymentMethod = (PaymentMethod)pm, IsSelected = false });
                }
            }
            return result;
        }

        private HashSet<Specialization> CreateSpecializationHashSet(List<SpecializationModel> specializations)
        {
            var result = new HashSet<Specialization>();
            foreach (var sp in specializations)
            {
                if (sp.IsSelected)
                {
                    result.Add(sp.Specialization);
                }
            }
            return result;
        }

        private HashSet<PaymentMethod> CreatePaymentMethodHashSet(List<PaymentMethodModel> paymentMethods)
        {
            var result = new HashSet<PaymentMethod>();
            foreach (var sp in paymentMethods)
            {
                if (sp.IsSelected)
                {
                    result.Add(sp.PaymentMethod);
                }
            }
            return result;
        }

        private async Task<int> CreateSpecialization(Restaurant restaurant, Specialization specialization)
        {
            var specializationDTO = new SpecializationDTO()
            {
                RestaurantId = restaurant.Id,
                Specialization = specialization,
            };
            var addResult = await _dbContext.Specializations.AddAsync(specializationDTO);

            return addResult.Entity.Id;
        }

        private void DeleteSpecialization(Restaurant restaurant, Specialization specialization)
        {
            var specializationDTO = new SpecializationDTO()
            {
                RestaurantId = restaurant.Id,
                Specialization = specialization,
            };
            specializationDTO = _dbContext.Specializations.Where(r => r.RestaurantId == restaurant.Id).Where(sp => sp.Specialization == specialization).FirstOrDefault();
            _dbContext.Specializations.Remove(specializationDTO);
        }

        private async Task<int> CreatePaymentMethod(RestaurantAdditionalInfo restaurantAdditionalInfo, PaymentMethod paymentMethod)
        {
            var paymentMethodDTO = new PaymentMethodDTO
            {
                RestaurantAdditionalInfoId = restaurantAdditionalInfo.Id,
                PaymentMethod = paymentMethod,
            };
            var addResult = await _dbContext.PaymentMethods.AddAsync(paymentMethodDTO);
            return addResult.Entity.Id;
        }

        private void DeletePaymentMethod(RestaurantAdditionalInfo restaurantAdditionalInfo, PaymentMethod paymentMethod)
        {
            var paymentMethodDTO = new PaymentMethodDTO
            {
                RestaurantAdditionalInfoId = restaurantAdditionalInfo.Id,
                PaymentMethod = paymentMethod,
            };
            paymentMethodDTO = _dbContext.PaymentMethods.Where(r => r.RestaurantAdditionalInfoId == restaurantAdditionalInfo.Id).Where(sp => sp.PaymentMethod == paymentMethod).FirstOrDefault();
            _dbContext.PaymentMethods.Remove(paymentMethodDTO);
        }

        private string TimeSpanToString(TimeSpan? timeSpan)
        {
            if (timeSpan.HasValue)
            {
                string hoursStr = timeSpan.Value.Hours < 10 ? "0" + timeSpan.Value.Hours : timeSpan.Value.Hours.ToString();
                string minutesStr = timeSpan.Value.Minutes < 10 ? "0" + timeSpan.Value.Minutes : timeSpan.Value.Minutes.ToString();
                return hoursStr + ":" + minutesStr;
            }
            else
                return "";
        }

        private TimeSpan? StringToTimeSpan(string time)
        {
            if (time != null)
            {
                int hours = Int32.Parse(time.Substring(0, 2));
                int minutes = Int32.Parse(time.Substring(3, 2));
                return new TimeSpan(hours, minutes, 0);
            }
            else
                return null;
        }

        private ProductGroup CreateProductGroup(ProductGroupModel model, Restaurant restaurant)
        {
            var newProductGroup = new ProductGroup
            {
                Name = model.Name,
                RestaurantId = restaurant.Id,
            };
            var newProductGroupDTO = _dbContext.ProductGroups.Add(_mapper.Map<ProductGroup, ProductGroupDTO>(newProductGroup));
            _dbContext.SaveChanges();

            var portionPrices = CreatePortionPrices(model.PortionPrices);
            var portionPricesProductGroup = CreatePortionPricesProductGroup(portionPrices, _mapper.Map<ProductGroupDTO, ProductGroup>(newProductGroupDTO.Entity));

            newProductGroup.PortionPrices = portionPricesProductGroup.ToHashSet();
            return newProductGroup;
        }

        private ICollection<PortionPrice> CreatePortionPrices(IEnumerable<PortionPriceModel> portionPriceModels)
        {
            if (portionPriceModels != null)
            {
                var portionPrices = new List<PortionPrice>();
                foreach (var pp in portionPriceModels)
                {
                    var portionPrice = new PortionPrice
                    {
                        Portion = pp.Portion,
                        Price = pp.Price
                    };
                    portionPrices.Add(portionPrice);
                }
                return portionPrices;
            }
            return null;
        }

        private ICollection<PortionPriceProductGroup> CreatePortionPricesProductGroup(IEnumerable<PortionPrice> portionPrices, ProductGroup productGroup)
        {
            var portionPricesProductGroup = new List<PortionPriceProductGroup>();
            foreach (var pp in portionPrices)
            {
                var newPortionPriceProductGroup = new PortionPriceProductGroup
                {
                    PortionPriceId = pp.Id,
                    PortionPrice = pp,
                    ProductGroupId = productGroup.Id,
                };
                portionPricesProductGroup.Add(newPortionPriceProductGroup);
            }

            var portionPricesProductGroupDTO = new List<PortionPriceProductGroupsDTO>();
            foreach (var pppg in portionPricesProductGroup)
            {
                portionPricesProductGroupDTO.Add(_mapper.Map<PortionPriceProductGroup, PortionPriceProductGroupsDTO>(pppg));
            }
            _dbContext.PortionPriceProductGroups.AddRange(portionPricesProductGroupDTO);

            var result = new List<PortionPriceProductGroup>();
            foreach (var pppg in portionPricesProductGroupDTO)
            {
                result.Add(_mapper.Map<PortionPriceProductGroupsDTO, PortionPriceProductGroup>(pppg));
            }

            return result;
        }

        private ICollection<ProductGroup> GetProductGroupsFull(Restaurant restaurant)
        {
            var productGroupsDTO = _dbContext.ProductGroups
                .Where(pg => pg.RestaurantId == restaurant.Id)
                .ToList();

            foreach (var pgdto in productGroupsDTO)
            {
                pgdto.PortionPrices = _dbContext.PortionPriceProductGroups
                    .Where(pppg => pppg.ProductGroupId == pgdto.Id)
                    .Include(pp => pp.PortionPrice)
                    .ToHashSet();
            }

            var productGroups = new List<ProductGroup>();
            foreach (var pgdto in productGroupsDTO)
            {
                productGroups.Add(_mapper.Map<ProductGroupDTO, ProductGroup>(pgdto));
            }

            return productGroups;
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

        private ProductGroupDTO GetProductGroupDTOById(int id)
        {
            var result = _dbContext.ProductGroups.Where(pg => pg.Id == id)
                .FirstOrDefault();
            result.PortionPrices = _dbContext.PortionPriceProductGroups.
                Where(pp => pp.ProductGroupId == result.Id)
                .Include(pp => pp.PortionPrice)
                .ToHashSet();
            return result;
        }
    }
}
