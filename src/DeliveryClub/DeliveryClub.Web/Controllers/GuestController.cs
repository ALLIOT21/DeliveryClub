﻿using DeliveryClub.Domain.AuxiliaryModels.Guest;
using DeliveryClub.Domain.Logic.Interfaces;
using DeliveryClub.Infrastructure.Mapping;
using DeliveryClub.Web.ViewModels.Guest;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DeliveryClub.Web.Controllers
{
    public class GuestController : Controller
    {
        private readonly IGuestService _guestService;
        private readonly Mapper _mapper;
        private const int PAGE_SIZE = 9;
                
        public GuestController(IGuestService guestService)
        {
            _guestService = guestService;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
        }

        public IActionResult Index(int page = 1)
        {
            var currentUserRole = _guestService.GetUserRole();
            if (currentUserRole != null)
            {
                return RedirectToAction("Index", currentUserRole);
            }
            else
            {              
                ICollection<RestaurantPartialViewModel> rpvms = new List<RestaurantPartialViewModel>();
                foreach (var rpm in _guestService.GetRestaurantsPartially())
                {
                    rpvms.Add(_mapper.Map<RestaurantPartialModel, RestaurantPartialViewModel>(rpm));
                }

                var count = rpvms.Count;
                var items = rpvms.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE).ToList();

                PageViewModel pageViewModel = new PageViewModel(count, page, PAGE_SIZE);
                IndexViewModel viewModel = new IndexViewModel
                {
                    PageViewModel = pageViewModel,
                    RestaurantPartialViewModels = items
                };
                
                return View(viewModel);
            }
        }

        [Route("[controller]/[action]/{id}")]
        public IActionResult Restaurant(int id)
        {
            var restaurantViewModel = _mapper.Map<RestaurantFullModel, RestaurantFullViewModel>(_guestService.GetRestaurantFull(id));
            return View(restaurantViewModel);
        }

        [HttpGet]
        public IActionResult CreateOrder()
        {         
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var orderId = await _guestService.CreateOrder(_mapper.Map<CreateOrderViewModel, CreateOrderModel>(model));
                return View("OrderSent", orderId);
            }            
            return View(model);
        }
    }
}
