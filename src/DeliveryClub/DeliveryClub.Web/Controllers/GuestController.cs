using Microsoft.AspNetCore.Mvc;
using DeliveryClub.Domain.Logic.Interfaces;
using DeliveryClub.Domain.Logic.Services;

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
            return View();
        }
    }
}
