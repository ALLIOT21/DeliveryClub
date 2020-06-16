using DeliveryClub.Domain.AuxiliaryModels.Admin;
using DeliveryClub.Domain.Logic.Interfaces;
using DeliveryClub.Infrastructure;
using DeliveryClub.Infrastructure.Extensions;
using DeliveryClub.Infrastructure.Mapping;
using DeliveryClub.Infrastructure.Notifications;
using DeliveryClub.Infrastructure.Validation;
using DeliveryClub.Web.ViewModels.Admin.Dispatchers;
using DeliveryClub.Web.ViewModels.Admin.Info;
using DeliveryClub.Web.ViewModels.Admin.Menu;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
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

            this.AddNotificationToViewBag(TempData.GetNotificationMessage());

            return View(restaurantInfoViewModel);
        }

        public async Task<IActionResult> Menu()
        {
            var productGroups = await _adminService.GetProductGroups();
            var productGroupsView = new List<ProductGroupViewModel>();

            foreach (var pg in productGroups)
            {
                productGroupsView.Add(_mapper.Map<ProductGroupModel, ProductGroupViewModel>(pg));
            }

            this.AddNotificationToViewBag(TempData.GetNotificationMessage());

            return View(productGroupsView);
        }

        public async Task<IActionResult> Dispatchers()
        {
            var gdms = await _adminService.GetDispatchers();
            var gdvms = new List<GetDispatcherViewModel>();
            foreach (var gdm in gdms)
            {
                gdvms.Add(_mapper.Map<GetDispatcherModel, GetDispatcherViewModel>(gdm));
            }

            this.AddNotificationToViewBag(TempData.GetNotificationMessage());

            return View(gdvms);
        }

        [HttpGet]
        public IActionResult CreateDispatcher()
        {
            this.AddNotificationToViewBag(TempData.GetNotificationMessage());

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateDispatcher(CreateDispatcherViewModel model)
        {
            if (ModelState.IsValid)
            {
                var createResult = await _adminService.CreateDispatcher(_mapper.Map<CreateDispatcherViewModel, CreateDispatcherModel>(model));
                if (createResult.Succeeded)
                {
                    TempData.AddNotificationMessage(new Notification
                    {
                        Type = NotificationType.Success,
                        Message = "Dispatcher succesfully created!"
                    });
                    return RedirectToAction(nameof(CreateDispatcher));
                }
                AddModelErrors(ModelState, createResult);
                this.AddNotificationToViewBag(new Notification
                {
                    Message = "Dispatcher has not been created!",
                    Type = NotificationType.Warning
                });
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult UpdateDispatcher(int id)
        {
            var d = _adminService.GetDispatcher(id);
            this.AddNotificationToViewBag(TempData.GetNotificationMessage());

            return View(_mapper.Map<UpdateDispatcherModel, UpdateDispatcherViewModel>(d));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDispatcher(UpdateDispatcherViewModel model)
        {
            if (ModelState.IsValid)
            {
                var iresult = await _adminService.UpdateDispatcher(_mapper.Map<UpdateDispatcherViewModel, UpdateDispatcherModel>(model));
                if (iresult.Succeeded)
                {
                    TempData.AddNotificationMessage(new Notification
                    {
                        Type = NotificationType.Success,
                        Message = "Dispatcher succesfully updated!"
                    });
                    return RedirectToAction(nameof(Dispatchers));
                }
                AddModelErrors(ModelState, iresult);
                this.AddNotificationToViewBag(new Notification
                {
                    Message = "Dispatcher is not updated",
                    Type = NotificationType.Warning
                });
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteDispatcher(int id)
        {
            await _adminService.DeleteDispatcher(id);

            TempData.AddNotificationMessage(new Notification
            {
                Type = NotificationType.Success,
                Message = "Dispatcher succesfully deleted!"
            });

            return RedirectToAction(nameof(Dispatchers));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductGroup(ProductGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                var newProductGroup = await _adminService.CreateProductGroup(_mapper.Map<ProductGroupViewModel, ProductGroupModel>(model));
                var productGroupView = _mapper.Map<ProductGroupModel, ProductGroupViewModel>(newProductGroup);
                TempData.AddNotificationMessage(new Notification
                {
                    Message = "Product group successfully created!",
                    Type = NotificationType.Success
                });
            }     
            else
            {
                TempData.AddNotificationMessage(new Notification
                {
                    Type = NotificationType.Warning,
                    Message = "Product group is not created!"
                });
            }
            
            return RedirectToAction(nameof(Menu));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRestaurant(RestaurantInfoViewModel model)
        {
            var updatedRestaurantInfo = await _adminService.UpdateRestaurantInfo(_mapper.Map<RestaurantInfoViewModel, RestaurantInfoModel>(model));
            TempData.AddNotificationMessage(new Notification
            {
                Type = NotificationType.Success,
                Message = "Restaurant info successfully updated!"
            });

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductGroup(int id)
        {
            await _adminService.DeleteProductGroup(id);

            TempData.AddNotificationMessage(new Notification
            {
                Type = NotificationType.Success,
                Message = "Product successfully deleted!"
            });

            return RedirectToAction(nameof(Menu));
        }
        [HttpPost]
        public async Task<IActionResult> UpdateProductGroup(ProductGroupViewModel model)
        {
            await _adminService.UpdateProductGroup(_mapper.Map<ProductGroupViewModel, ProductGroupModel>(model));

            TempData.AddNotificationMessage(new Notification
            {
                Type = NotificationType.Success,
                Message = "Product group successfully updated"
            });

            return RedirectToAction(nameof(Menu));
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.Image != null)
                {
                    var imageValidator = new ImageValidator();
                    if (imageValidator.Validate(model.Image))
                    {
                        await _adminService.CreateProduct(_mapper.Map<ProductViewModel, ProductModel>(model));

                        TempData.AddNotificationMessage(new Notification
                        {
                            Type = NotificationType.Success,
                            Message = "Product successfully created!"
                        });
                        return RedirectToAction(nameof(CreateProduct));
                    }
                    else
                    {
                        ModelState.AddModelError("Image", "File is not an image.");
                    }
                }
                else
                {
                    await _adminService.CreateProduct(_mapper.Map<ProductViewModel, ProductModel>(model));

                    TempData.AddNotificationMessage(new Notification
                    {
                        Type = NotificationType.Success,
                        Message = "Product successfully created!"
                    });

                    return RedirectToAction(nameof(CreateProduct));
                }
            }
            var productGroupModels = await _adminService.GetProductGroups();
            var productGroupViewModels = CreateProductGroupViewModels(productGroupModels);
            var productGroupNames = GetProductGroupNames(productGroupViewModels);

            ViewBag.ProductGroupNames = productGroupNames;

            this.AddNotificationToViewBag(new Notification
            {
                Message = "Product is not created!",
                Type = NotificationType.Warning
            });

            return View(model);
        }    

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {            
            var productGroupModels = await _adminService.GetProductGroups();
            var productGroupViewModels = CreateProductGroupViewModels(productGroupModels);
            var productGroupNames = GetProductGroupNames(productGroupViewModels);

            ViewBag.ProductGroupNames = productGroupNames;
            this.AddNotificationToViewBag(TempData.GetNotificationMessage());

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

            this.AddNotificationToViewBag(TempData.GetNotificationMessage());

            return View(productViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct(ProductViewModel productViewModel)
        {
            if (Convert.ToBoolean(productViewModel.ProductGroupPortionPriced))
            {
                productViewModel.PortionPrices = null;
            }

            var updatedProduct = await _adminService.UpdateProduct(_mapper.Map<ProductViewModel, ProductModel>(productViewModel));
            
            TempData.AddNotificationMessage(new Notification
            {
                Type = NotificationType.Success,
                Message = "Product successfully updated!"
            });

            return RedirectToAction(nameof(UpdateProduct), new { id = updatedProduct.Id });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _adminService.DeleteProduct(id);

            this.AddNotificationToViewBag(new Notification
            {
                Type = NotificationType.Success,
                Message = "Product successfully deleted!"
            });

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

        private void AddModelErrors(ModelStateDictionary modelState, IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                modelState.AddModelError(string.Empty, error.Description);
            }
        }        
    }
}