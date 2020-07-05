using DeliveryClub.Domain.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryClub.Web.ViewModels.Dispatchers
{
    public class GetRestaurantOrderViewModel
    {
        public string Name { get; set; }

        public double DeliveryCost { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public ICollection<GetOrderedProductViewModel> Products { get; set; }        
    }
}
