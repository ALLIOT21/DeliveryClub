using System.Collections.Generic;

namespace DeliveryClub.Web.ViewModels.Guest
{
    public class CreateOrderViewModel
    {
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public AddressViewModel DeliveryAddress { get; set; }

        public string Comment { get; set; }

        public ICollection<RestaurantOrderViewModel> Orders { get; set; }
    }
}
