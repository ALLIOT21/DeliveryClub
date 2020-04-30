using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using DeliveryClub.Domain.AuxiliaryModels.Admin;
using DeliveryClub.Domain.Logic.Interfaces;
using DeliveryClub.Domain.Models.Enumerations;
using DeliveryClub.Infrastructure.Mapping;
using DeliveryClub.Web.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DeliveryClub.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly Mapper _mapper;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
        }

        public async Task<IActionResult> Index()
        {
            RestaurantInfoViewModel restaurantInfoMock = new RestaurantInfoViewModel
            {
                Name = "2 берега",
                Specializations = new List<SpecializationViewModel> {
                    new SpecializationViewModel { Specialization = Specialization.Pizza, IsSelected = true },
                    new SpecializationViewModel { Specialization = Specialization.FastFood, IsSelected = false },
                    new SpecializationViewModel { Specialization = Specialization.Sushi, IsSelected = false },
                },
                DeliveryCost = 0,
                MinimalOrderPrice = 500,
                Description = "Горячая пицца, свежие роллы или сочные бургеры для перекуса без хлопот дома или в офисе. Широкий ассортимент простых и понятных блюд европейской и паназиатской кухонь, приготовленных из свежайших продуктов по традиционным рецептам.",
                PaymentMethods = new List<PaymentMethodViewModel> {
                    new PaymentMethodViewModel { PaymentMethod = PaymentMethod.Card, IsSelected = true },
                    new PaymentMethodViewModel { PaymentMethod = PaymentMethod.Cash, IsSelected = false },
                },
                DeliveryMaxTime = "45",
                OrderTimeBegin = "08:30",
                OrderTimeEnd = "22:30",
            };
            var restaurantInfoModel = await _adminService.GetRestaurantInfo(HttpContext.User);
            var restaurantInfoViewModel = _mapper.Map<RestaurantInfoModel, RestaurantInfoViewModel>(restaurantInfoModel);
            return View(restaurantInfoViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRestaurant(RestaurantInfoViewModel model)
        {
            var updatedRestaurantInfo = await _adminService.UpdateRestaurantInfo(HttpContext.User, _mapper.Map<RestaurantInfoViewModel, RestaurantInfoModel>(model));
            return await Index();
        }
    }
}