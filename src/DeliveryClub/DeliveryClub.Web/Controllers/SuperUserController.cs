using DeliveryClub.Domain.AuxiliaryModels;
using DeliveryClub.Domain.Logic.Interfaces;
using DeliveryClub.Domain.Models.Actors;
using DeliveryClub.Infrastructure.Mapping;
using DeliveryClub.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
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
            return View(GetAdminsView(_superUserService.GetAdmins()));
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
                var result = await _superUserService.CreateAdminAndRestaurant(_mapper.Map<CreateAdminViewModel, CreateAdminModel>(model));
                if (result.Succeeded)
                {
                    return RedirectToAction("CreateAdmin");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }                
            }
            return View(model);                        
        }

        public IActionResult UpdateAdmin()
        {
            return View();
        }

        public IActionResult DeleteAdmin()
        {
            return View();
        }        

        private IEnumerable<GetAdminViewModel> GetAdminsView(IEnumerable<Admin> admins)
        {
            var result = new List<GetAdminViewModel>();
            foreach (var admin in admins)
            {
                var adminView = new GetAdminViewModel();
                adminView.Email = admin.User.Email;
                adminView.RestaurantName = admin.Restaurant.Name;
                result.Add(adminView);
            }
            return result;
        }
    }
}