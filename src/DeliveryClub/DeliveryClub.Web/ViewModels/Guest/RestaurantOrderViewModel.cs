using DeliveryClub.Domain.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryClub.Web.ViewModels.Guest
{
    public class RestaurantOrderViewModel
    {
        public int RestaurantId { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public ICollection<OrderedProductViewModel> Products { get; set; }
    }
}
