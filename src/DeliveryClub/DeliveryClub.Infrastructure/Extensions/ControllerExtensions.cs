using Microsoft.AspNetCore.Mvc;

namespace DeliveryClub.Infrastructure.Extensions
{
    public static class ControllerExtensions
    {
        public static void AddNotificationToViewBag(this Controller controller, Notification notification)
        {
            controller.ViewBag.Notification = notification;
        }
    }
}
