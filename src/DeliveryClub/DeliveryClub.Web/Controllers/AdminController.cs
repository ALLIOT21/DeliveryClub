using DeliveryClub.Domain.AuxiliaryModels.Admin;
using DeliveryClub.Domain.Logic.Interfaces;
using DeliveryClub.Infrastructure.Mapping;
using DeliveryClub.Web.ViewModels.Admin;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace DeliveryClub.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly Mapper _mapper;

        public AdminController(IAdminService adminService,
                               IWebHostEnvironment hostingEnvironment)
        {
            _adminService = adminService;
            _hostingEnvironment = hostingEnvironment;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
        }

        public async Task<IActionResult> Index()
        {
            var restaurantInfoModel = await _adminService.GetRestaurantInfo(HttpContext.User);
            var restaurantInfoViewModel = _mapper.Map<RestaurantInfoModel, RestaurantInfoViewModel>(restaurantInfoModel);
            return View(restaurantInfoViewModel);
        }

        public async Task<IActionResult> Menu(ProductGroupViewModel model)
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
            if (ModelState.IsValid)
            {
                var newProductGroup = await _adminService.CreateProductGroup(HttpContext.User, _mapper.Map<ProductGroupViewModel, ProductGroupModel>(model));
                var productGroupView = _mapper.Map<ProductGroupModel, ProductGroupViewModel>(newProductGroup);
            }
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

        public async Task<IActionResult> UpdateProductGroup(ProductGroupViewModel model)
        {
            await _adminService.UpdateProductGroup(HttpContext.User, _mapper.Map<ProductGroupViewModel, ProductGroupModel>(model));
            return RedirectToAction(nameof(Menu));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductViewModel model)
        {
            model.ImageFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "resources\\img\\");
            await _adminService.CreateProduct(HttpContext.User, _mapper.Map<ProductViewModel, ProductModel>(model));
            return RedirectToAction(nameof(CreateProduct));
        }

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            var productGroupModels = await _adminService.GetProductGroups(HttpContext.User);
            var productGroupViewModels = new List<ProductGroupViewModel>();
            foreach (var pgm in productGroupModels)
            {
                productGroupViewModels.Add(_mapper.Map<ProductGroupModel, ProductGroupViewModel>(pgm));
            }

            var productGroupNames = GetProductGroupNames(productGroupViewModels);

            ViewBag.ProductGroupNames = productGroupNames;
            return View("Product");
        }

        private ICollection<string> GetProductGroupNames(ICollection<ProductGroupViewModel> productGroupViewModels)
        {         
            var productGroupNames = new List<string>();
            foreach (var pgvm in productGroupViewModels)
            {
                productGroupNames.Add(pgvm.Name);
            }
            return productGroupNames;
        }

        private ICollection<string> GetPortionPricePortions(ICollection<PortionPriceModel> portionPriceModels)
        {
            var result = new List<string>();
            foreach (var ppm in portionPriceModels)
            {
                result.Add(ppm.Portion);
            }
            return result;
        }

        
}
}