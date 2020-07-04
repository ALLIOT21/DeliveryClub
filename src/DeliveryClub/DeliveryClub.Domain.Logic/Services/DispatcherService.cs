using DeliveryClub.Domain.AuxiliaryModels.Dispatcher;
using DeliveryClub.Domain.Logic.Extensions;
using DeliveryClub.Domain.Logic.Interfaces;
using DeliveryClub.Domain.Logic.Managers;
using DeliveryClub.Domain.Logic.Mapping;
using DeliveryClub.Domain.Models.Actors;
using DeliveryClub.Domain.Models.Enumerations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeliveryClub.Domain.Logic.Services
{
    public class DispatcherService : IDispatcherService
    {
        private readonly OrderManager _orderManager;
        private readonly AuxiliaryMapper _auxiliaryMapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DispatcherManager _dispatcherManager;

        public DispatcherService(OrderManager orderManager,
                                 AuxiliaryMapper auxiliaryMapper,
                                 IHttpContextAccessor httpContextAccessor,
                                 UserManager<IdentityUser> userManager,
                                 DispatcherManager dispatcherManager)
        {
            _orderManager = orderManager;
            _auxiliaryMapper = auxiliaryMapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _dispatcherManager = dispatcherManager;
        }

        public async Task<ICollection<DispatcherOrderModel>> GetOrders(OrderStatus orderStatus)
        {
            var disp = await GetCurrentDispatcher();

            var doms = new List<DispatcherOrderModel>();
            foreach (var order in _orderManager.GetOrders(orderStatus, disp.Id))
            {
                doms.Add(_auxiliaryMapper.CreateDispatcherOrderModel(order));
            }

            return doms;
        }

        public DispatcherOrderFullModel GetOrder(int id)
        {
            return null;
        }
        
        private async Task<Dispatcher> GetCurrentDispatcher()
        {            
            var currentIdentityUser = await _userManager.GetCurrentIdentityUser(_httpContextAccessor.HttpContext.User);
            return _dispatcherManager.GetDispatcher(currentIdentityUser.Id);      
        }
    }
}
