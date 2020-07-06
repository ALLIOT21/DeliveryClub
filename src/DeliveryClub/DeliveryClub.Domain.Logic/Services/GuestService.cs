using DeliveryClub.Domain.AuxiliaryModels.Guest;
using DeliveryClub.Domain.Logic.Interfaces;
using DeliveryClub.Domain.Logic.Managers;
using DeliveryClub.Domain.Logic.Mapping;
using DeliveryClub.Domain.Models;
using DeliveryClub.Infrastructure.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
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
        private readonly OrderManager _orderManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHubContext<DispatcherNotificationHub> _hubContext;
        private readonly DispatcherManager _dispatcherManager;

        public GuestService(SignInManager<IdentityUser> signInManager,
                            RestaurantManager restaurantManager,
                            AuxiliaryMapper auxiliaryMapper,
                            RestaurantAdditionalInfoManager restaurantAdditionalInfoManager,
                            OrderManager orderManager,
                            IHttpContextAccessor httpContextAccessor,
                            IHubContext<DispatcherNotificationHub> hubContext,
                            DispatcherManager dispatcherManager)
        {
            _signInManager = signInManager;
            _restaurantManager = restaurantManager;
            _auxiliaryMapper = auxiliaryMapper;
            _restaurantAdditionalInfoManager = restaurantAdditionalInfoManager;
            _orderManager = orderManager;
            _httpContextAccessor = httpContextAccessor;
            _hubContext = hubContext;
            _dispatcherManager = dispatcherManager;
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
            var order = await _orderManager.CreateOrder(model);

            var group = _hubContext.Clients.Group(_dispatcherManager.GetDispatcher(order.DispatcherId).User.Email);
            await group.SendAsync("ReceiveOrder", "New Order!");

            return order.Id;
        }

        private ClaimsPrincipal GetUserClaims()
        {
            return _signInManager.Context.User;
        }
    }
}