using Microsoft.AspNetCore.Mvc;

namespace DeliveryClub.Web.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}