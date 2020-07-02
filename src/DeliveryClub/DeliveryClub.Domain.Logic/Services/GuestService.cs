using DeliveryClub.Domain.AuxiliaryModels.Guest;
using DeliveryClub.Domain.Logic.Interfaces;
using DeliveryClub.Domain.Logic.Managers;
using DeliveryClub.Domain.Logic.Mapping;
using DeliveryClub.Domain.Models;
using DeliveryClub.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DeliveryClub.Domain.Logic.Services
{
    public class GuestService : IGuestService
    {
        private readonly RestaurantManager _restaurantManager;
        private readonly AuxiliaryMapper _auxiliaryMapper;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RestaurantAdditionalInfoManager _restaurantAdditionalInfoManager;
        private readonly PaymentMethodManager _paymentMethodManager;
        private readonly OrderManager _orderManager;

        public GuestService(SignInManager<IdentityUser> signInManager,
                            RestaurantManager restaurantManager,
                            AuxiliaryMapper auxiliaryMapper,
                            RestaurantAdditionalInfoManager restaurantAdditionalInfoManager,
                            PaymentMethodManager paymentMethodManager, 
                            OrderManager orderManager)
        {
            _signInManager = signInManager;
            _restaurantManager = restaurantManager;
            _auxiliaryMapper = auxiliaryMapper;
            _restaurantAdditionalInfoManager = restaurantAdditionalInfoManager;
            _paymentMethodManager = paymentMethodManager;
            _orderManager = orderManager;
        }

        public ICollection<RestaurantPartialModel> GetRestaurantsPartially()
        {
            var restaurants = _restaurantManager.GetRestaurantsPartially();

            var rpartials = _auxiliaryMapper.CreateRestaurantPartialModels(restaurants);
            return rpartials;
        }

        public RestaurantFullModel GetRestaurantFull(int id)
        {
            var restaurantFull = _restaurantManager.GetRestaurantFull(id);
            return _auxiliaryMapper.CreateRestaurantFullModel(restaurantFull);
        }

        public List<NamePaymentModel> GetRestaurantNamePayments(List<int> restaurantIds)
        {
            var result = new List<NamePaymentModel>();
            foreach (var rid in restaurantIds)
            {
                var raiId = _restaurantAdditionalInfoManager.GetRestaurantAdditionalInfoId(rid);
                var name = _restaurantManager.GetRestaurant(rid).Name;

                var npm = new NamePaymentModel
                {
                    Name = name,
                    PaymentMethods = _auxiliaryMapper.CreatePaymentMethodModelList(_paymentMethodManager.GetPaymentMethods(raiId))
                };

                result.Add(npm);
            }
            return result;            
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

        public async Task<int> CreateOrder(CreateOrderModel model)
        {
            var orderId = await _orderManager.CreateOrder(model);
            return orderId;
        }

        private ClaimsPrincipal GetUserClaims()
        {
            return _signInManager.Context.User;
        }
    }
}
