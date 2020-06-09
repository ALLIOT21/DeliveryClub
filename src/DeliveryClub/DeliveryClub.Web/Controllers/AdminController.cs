using DeliveryClub.Domain.AuxiliaryModels.Admin;
using DeliveryClub.Domain.Logic.Interfaces;
using DeliveryClub.Infrastructure.Mapping;
using DeliveryClub.Infrastructure.Validation;
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
            var restaurantInfoModel = await _adminService.GetRestaurantInfo();
            var restaurantInfoViewModel = _mapper.Map<RestaurantInfoModel, RestaurantInfoViewModel>(restaurantInfoModel);
            return View(restaurantInfoViewModel);
        }

        public async Task<IActionResult> Menu(ProductGroupViewModel model)
        {
            var productGroups = await _adminService.GetProductGroups();
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
                var newProductGroup = await _adminService.CreateProductGroup(_mapper.Map<ProductGroupViewModel, ProductGroupModel>(model));
                var productGroupView = _mapper.Map<ProductGroupModel, ProductGroupViewModel>(newProductGroup);
            }
            return RedirectToAction(nameof(Menu));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRestaurant(RestaurantInfoViewModel model)
        {
            var updatedRestaurantInfo = await _adminService.UpdateRestaurantInfo(_mapper.Map<RestaurantInfoViewModel, RestaurantInfoModel>(model));
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductGroup(int id)
        {
            await _adminService.DeleteProductGroup(id);
            return RedirectToAction(nameof(Menu));
        }

        public async Task<IActionResult> UpdateProductGroup(ProductGroupViewModel model)
        {
            await _adminService.UpdateProductGroup(_mapper.Map<ProductGroupViewModel, ProductGroupModel>(model));
            return RedirectToAction(nameof(Menu));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductViewModel model)
        {
            var imageValidator = new ImageValidator();
            if (imageValidator.Validate(model.Image))
            {
                await _adminService.CreateProduct(_mapper.Map<ProductViewModel, ProductModel>(model));
                return RedirectToAction(nameof(CreateProduct));
            }
            ModelState.AddModelError(string.Empty, "File is not an image.");

            return RedirectToAction(nameof(CreateProduct), model);
        }

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
            var productGroupModels = await _adminService.GetProductGroups();
            var productGroupViewModels = CreateProductGroupViewModels(productGroupModels);
            var productGroupNames = GetProductGroupNames(productGroupViewModels);

            ViewBag.ProductGroupNames = productGroupNames;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            var productModel = _adminService.GetProduct(id);
            var productViewModel = _mapper.Map<ProductModel, ProductViewModel>(productModel);

            var productGroupModels = await _adminService.GetProductGroups();
            var productGroupViewModels = CreateProductGroupViewModels(productGroupModels);
            var productGroupNames = GetProductGroupNames(productGroupViewModels);

            ViewBag.ProductGroupNames = productGroupNames;

            return View(productViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _adminService.DeleteProduct(id);
            return RedirectToAction(nameof(Menu));
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

        private ICollection<ProductGroupViewModel> CreateProductGroupViewModels(ICollection<ProductGroupModel> productGroupModels)
        {
            var productGroupViewModels = new List<ProductGroupViewModel>();
            foreach (var pgm in productGroupModels)
            {
                productGroupViewModels.Add(_mapper.Map<ProductGroupModel, ProductGroupViewModel>(pgm));
            }
            return productGroupViewModels;
        }
    }
}