using Microsoft.AspNetCore.Mvc;

namespace DeliveryClub.Web.Controllers
{
    public class DispatcherController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateDispatcher()
        {
            return View();
        }
    }
}