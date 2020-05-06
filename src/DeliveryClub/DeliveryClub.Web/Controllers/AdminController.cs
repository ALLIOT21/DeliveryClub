using DeliveryClub.Domain.AuxiliaryModels.Admin;
using DeliveryClub.Domain.Logic.Interfaces;
using DeliveryClub.Infrastructure.Mapping;
using DeliveryClub.Web.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

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
            var restaurantInfoModel = await _adminService.GetRestaurantInfo(HttpContext.User);
            var restaurantInfoViewModel = _mapper.Map<RestaurantInfoModel, RestaurantInfoViewModel>(restaurantInfoModel);
            return View(restaurantInfoViewModel);
        }

        public async Task<IActionResult> Menu()
        {
            var productGroups = await _adminService.GetProductGroups(HttpContext.User);
            var productGroupsView = new List<ProductGroupViewModel>();
            foreach (var pg in productGroups)
            {
                productGroupsView.Add(_mapper.Map<ProductGroupModel, ProductGroupViewModel>(pg));
            }
            return View(productGroupsView);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductGroup(ProductGroupViewModel model)
        {
            var newProductGroup = await _adminService.CreateProductGroup(HttpContext.User, _mapper.Map<ProductGroupViewModel, ProductGroupModel>(model));
            var productGroupView = _mapper.Map<ProductGroupModel, ProductGroupViewModel>(newProductGroup);
            return RedirectToAction(nameof(Menu));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRestaurant(RestaurantInfoViewModel model)
        {
            var updatedRestaurantInfo = await _adminService.UpdateRestaurantInfo(HttpContext.User, _mapper.Map<RestaurantInfoViewModel, RestaurantInfoModel>(model));
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductGroup(int id)
        {
            await _adminService.DeleteProductGroup(HttpContext.User, id);
            return RedirectToAction(nameof(Menu));
        }
    }
}