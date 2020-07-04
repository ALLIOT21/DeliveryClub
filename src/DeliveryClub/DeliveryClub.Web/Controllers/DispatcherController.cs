using DeliveryClub.Domain.AuxiliaryModels.Dispatcher;
using DeliveryClub.Domain.Logic.Interfaces;
using DeliveryClub.Domain.Models.Enumerations;
using DeliveryClub.Infrastructure.Mapping;
using DeliveryClub.Web.ViewModels.Dispatchers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace DeliveryClub.Web.Controllers
{
    public class DispatcherController : Controller
    {
        private readonly IDispatcherService _dispatcherService;
        private readonly Mapper _mapper;
        private const string REFERER_HEADER = "Referer";

        public DispatcherController(IDispatcherService dispatcherService)
        {
            _dispatcherService = dispatcherService;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
        }

        [Route("[controller]/Orders/Received")]
        public async Task<IActionResult> Index()
        {
            ViewBag.OrderStatus = OrderStatus.Received;
            return View(await GetOrders(OrderStatus.Received));
        }

        [Route("[controller]/[action]/{orderStatus}")]
        public async Task<ActionResult> Orders(OrderStatus orderStatus)
        {
            ViewBag.OrderStatus = orderStatus;
            return View(nameof(Index), await GetOrders(orderStatus));
        }

        [HttpPost]
        public async Task<IActionResult> SetOrderStatus(SetOrderStatusViewModel model)
        {
            await _dispatcherService.SetOrderStatus(model.Id, model.OrderStatus);
            return Redirect(Request.Headers[REFERER_HEADER].ToString());
        }

        private async Task<ICollection<DispatcherOrderViewModel>> GetOrders(OrderStatus orderStatus)
        {
            var dovms = new List<DispatcherOrderViewModel>();
            foreach (var dom in await _dispatcherService.GetOrders(orderStatus))
            {
                dovms.Add(_mapper.Map<DispatcherOrderModel, DispatcherOrderViewModel>(dom));
            }
            return dovms;
        }


    }
}