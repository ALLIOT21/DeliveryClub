using DeliveryClub.Domain.AuxiliaryModels;
using DeliveryClub.Domain.Logic.Interfaces;
using DeliveryClub.Infrastructure.Mapping;
using DeliveryClub.Web.ViewModels;
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

        private void AddModelErrors(ModelStateDictionary modelState, IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                modelState.AddModelError(string.Empty, error.Description);
            }
        }

    }
}