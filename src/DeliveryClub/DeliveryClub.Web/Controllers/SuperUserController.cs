using DeliveryClub.Domain.AuxiliaryModels.SuperUser;
using DeliveryClub.Domain.Logic.Interfaces;
using DeliveryClub.Infrastructure;
using DeliveryClub.Infrastructure.Extensions;
using DeliveryClub.Infrastructure.Mapping;
using DeliveryClub.Infrastructure.Notifications;
using DeliveryClub.Web.ViewModels.SuperUser;
using DeliveryClub.Web.ViewModels.SuperUser.Dispatchers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace DeliveryClub.Web.Controllers
{
    public class SuperUserController : Controller
    {
        private readonly ISuperUserService _superUserService;
        private readonly Mapper _mapper;

        public SuperUserController(ISuperUserService superUserService)
        {
            _superUserService = superUserService;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
        }

        public IActionResult Index()
        {
            var adminsView = new List<GetAdminViewModel>();
            foreach (var ad in _superUserService.GetAdmins())
            {
                adminsView.Add(_mapper.Map<GetAdminModel, GetAdminViewModel>(ad));
            }
            return View(adminsView);
        }

        [HttpGet]
        public IActionResult CreateAdmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdmin(CreateAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var createResult = await _superUserService.CreateAdminAndRestaurant(_mapper.Map<CreateAdminViewModel, CreateAdminModel>(model));
                if (createResult.Succeeded)
                {
                    return RedirectToAction("CreateAdmin");
                }                
                AddModelErrors(ModelState, createResult);                                             
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult UpdateAdmin(int id)
        {
            var admin = _superUserService.GetAdmin(id);
            return View(_mapper.Map<UpdateAdminModel, UpdateAdminViewModel>(admin.Result));
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateAdmin(UpdateAdminViewModel model)
        {
            if (ModelState.IsValid)
            {
                var updateResult = await _superUserService.UpdateAdmin(_mapper.Map<UpdateAdminViewModel, UpdateAdminModel>(model));
                if (updateResult.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }                
                AddModelErrors(ModelState, updateResult);                            
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAdmin(int id)
        {
            await _superUserService.DeleteAdminAndRestaurant(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Dispatchers()
        {
            var gdms = await _superUserService.GetDispatchers();
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
                var createResult = await _superUserService.CreateDispatcher(_mapper.Map<CreateDispatcherViewModel, CreateDispatcherModel>(model));
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
            var d = _superUserService.GetDispatcher(id);
            this.AddNotificationToViewBag(TempData.GetNotificationMessage());

            return View(_mapper.Map<UpdateDispatcherModel, UpdateDispatcherViewModel>(d));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDispatcher(UpdateDispatcherViewModel model)
        {
            if (ModelState.IsValid)
            {
                var iresult = await _superUserService.UpdateDispatcher(_mapper.Map<UpdateDispatcherViewModel, UpdateDispatcherModel>(model));
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
            await _superUserService.DeleteDispatcher(id);

            TempData.AddNotificationMessage(new Notification
            {
                Type = NotificationType.Success,
                Message = "Dispatcher succesfully deleted!"
            });

            return RedirectToAction(nameof(Dispatchers));
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