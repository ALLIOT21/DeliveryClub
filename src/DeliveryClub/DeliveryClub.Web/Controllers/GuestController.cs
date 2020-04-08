using DeliveryClub.Domain.Logic.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DeliveryClub.Web.Controllers
{
    public class GuestController : Controller
    {
        private readonly IGuestService _service;

        public GuestController(IGuestService service)
        {
            _service = service;            
        }


        // GET: /<controller>/
        public IActionResult Index()
        {
            var currentUserRole = _service.GetUserRole();
            if (currentUserRole != null)
            {
                return RedirectToAction("Index", currentUserRole);
            }
            else
                return View();
        }
    }
}
