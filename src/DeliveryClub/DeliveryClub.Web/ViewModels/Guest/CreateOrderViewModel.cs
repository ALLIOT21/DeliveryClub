using DeliveryClub.Domain.Models.Enumerations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeliveryClub.Web.ViewModels.Guest
{
    public class CreateOrderViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [RegularExpression("^(\\+375|80)(29|25|44|33)(\\d{3})(\\d{2})(\\d{2})$", ErrorMessage = "Your phone number is not correct.")]
        public string PhoneNumber { get; set; }
        
        public AddressViewModel DeliveryAddress { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        public string Comment { get; set; }

        public ICollection<RestaurantOrderViewModel> Orders { get; set; }
    }
}
