using Microsoft.AspNetCore.Mvc;
using DeliveryClub.Domain.Logic.Interfaces;
using DeliveryClub.Infrastructure.Mapping;
using DeliveryClub.Domain.Models.Enumerations;
using DeliveryClub.Web.ViewModels.Dispatchers;
using System.Collections.Generic;
using System.Reflection;
using DeliveryClub.Domain.AuxiliaryModels.Dispatcher;
using System.Threading.Tasks;

namespace DeliveryClub.Web.Controllers
{
    public class DispatcherController : Controller
    {
        private readonly IDispatcherService _dispatcherService;
        private readonly Mapper _mapper;
        public DispatcherController(IDispatcherService dispatcherService)
        {
            _dispatcherService = dispatcherService;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.OrderStatus = OrderStatus.Received;
            return View(await GetOrders(OrderStatus.Received));
        }

        public async new Task<IActionResult> Accepted()
        {
            ViewBag.OrderStatus = OrderStatus.Accepted;
            return View(nameof(Index), await GetOrders(OrderStatus.Accepted));
        }

        public async Task<IActionResult> Declined()
        {
            ViewBag.OrderStatus = OrderStatus.Declined;
            return View(nameof(Index), await GetOrders(OrderStatus.Declined));
        }

        public async Task<IActionResult> Courier()
        {
            ViewBag.OrderStatus = OrderStatus.Courier;
            return View(nameof(Index), await GetOrders(OrderStatus.Courier));
        }

        public async Task<IActionResult> Delivered()
        {
            ViewBag.OrderStatus = OrderStatus.Delivered;
            return View(nameof(Index), await GetOrders(OrderStatus.Delivered));
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