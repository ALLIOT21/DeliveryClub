using Microsoft.AspNetCore.Mvc;

namespace DeliveryClub.Web.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            switch (statusCode)
            {
                case 404:
                {
                    ViewBag.ErrorMessage = "404 Not Found";
                    break;
                }
                case 500:
                {
                    ViewBag.ErrorMessage = "500 Internal Server Error";
                    break;
                }
                default:
                {
                    ViewBag.ErrorMessage = "Unexpected error";
                    break;
                }
            }
            return View();
        }
    }
}
