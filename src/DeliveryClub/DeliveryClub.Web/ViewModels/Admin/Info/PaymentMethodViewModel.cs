using DeliveryClub.Domain.Models.Enumerations;

namespace DeliveryClub.Web.ViewModels.Admin.Info
{
    public class PaymentMethodViewModel
    {
        public PaymentMethod PaymentMethod { get; set; }

        public bool IsSelected { get; set; }
    }
}
