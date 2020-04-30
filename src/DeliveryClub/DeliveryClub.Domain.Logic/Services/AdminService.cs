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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
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

            var toAdd = new HashSet<Specialization>();
            var toDelete = new HashSet<Specialization>();
            foreach (var osp in oldSpecializations)
            {
                if (!newSpecializations.Contains(osp))
                {
                    toDelete.Add(osp);
                }
            }

            foreach (var nsp in newSpecializations)
            {
                if (!oldSpecializations.Contains(nsp))
                {
                    toAdd.Add(nsp);
                }
            }

            foreach (var ta in toAdd)
            {
                await CreateSpecialization(restaurant, ta);
            }

            foreach (var td in toDelete)
            {
                DeleteSpecialization(restaurant, td);
            }
        }

        private async Task UpdatePaymentMethods(RestaurantAdditionalInfo restaurantAdditionalInfo, HashSet<PaymentMethod> newPaymentMethods)
        {
            var oldPaymentMethod = GetPaymentMethods(restaurantAdditionalInfo);

            var toAdd = new HashSet<PaymentMethod>();
            var toDelete = new HashSet<PaymentMethod>();
            foreach (var osp in oldPaymentMethod)
            {
                if (!newPaymentMethods.Contains(osp))
                {
                    toDelete.Add(osp);
                }
            }

            foreach (var nsp in newPaymentMethods)
            {
                if (!oldPaymentMethod.Contains(nsp))
                {
                    toAdd.Add(nsp);
                }
            }

            foreach (var ta in toAdd)
            {
                await CreatePaymentMethod(restaurantAdditionalInfo, ta);
            }

            foreach (var td in toDelete)
            {
                DeletePaymentMethod(restaurantAdditionalInfo, td);
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
            var updateResult = _dbContext.Restaurants.Update(_mapper.Map<Restaurant, RestaurantDTO>(restaurant));
            await _dbContext.SaveChangesAsync();
            return _mapper.Map<RestaurantDTO, Restaurant>(updateResult.Entity);
            
        }

        private async Task<RestaurantAdditionalInfo> UpdateRestaurantAdditionalInfo(RestaurantAdditionalInfo restaurantAdditionalInfo, RestaurantInfoModel restaurantInfoModel)
        {
            restaurantAdditionalInfo.OrderTimeBegin = StringToTimeSpan(restaurantInfoModel.OrderTimeBegin);
            restaurantAdditionalInfo.OrderTimeEnd = StringToTimeSpan(restaurantInfoModel.OrderTimeEnd);
            await UpdatePaymentMethods(restaurantAdditionalInfo, CreatePaymentMethodHashSet(restaurantInfoModel.PaymentMethods));
            var updateResult = _dbContext.RestaurantAdditionalInfos.Update(_mapper.Map<RestaurantAdditionalInfo, RestaurantAdditionalInfoDTO>(restaurantAdditionalInfo));
            return _mapper.Map<RestaurantAdditionalInfoDTO, RestaurantAdditionalInfo>(updateResult.Entity);
        }

        private RestaurantInfoModel CreateRestaurantInfoModel(Restaurant restaurant)
        {
            var restaurantInfo = new RestaurantInfoModel()
            {
                Name = restaurant.Name,
                Specializations = CreateSpecializationList(restaurant.Specializations),
                DeliveryCost = restaurant.DeliveryCost,
                MinimalOrderPrice = restaurant.MinimalOrderPrice,
                Description = restaurant.RestaurantAdditionalInfo.Description,
                PaymentMethods = CreatePaymentMethodList(restaurant.RestaurantAdditionalInfo.PaymentMethods),
                DeliveryMaxTime = TimeSpanToString(restaurant.RestaurantAdditionalInfo.DeliveryMaxTime),
                OrderTimeBegin = TimeSpanToString(restaurant.RestaurantAdditionalInfo.OrderTimeBegin),
                OrderTimeEnd = TimeSpanToString(restaurant.RestaurantAdditionalInfo.OrderTimeEnd),
            };
            return restaurantInfo;
        }

        private List<SpecializationModel> CreateSpecializationList(HashSet<Specialization> specializations)
        {
            var result = new List<SpecializationModel>();
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
    }
}
