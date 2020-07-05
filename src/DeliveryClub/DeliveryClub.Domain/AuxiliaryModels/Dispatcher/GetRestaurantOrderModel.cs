using DeliveryClub.Domain.Models.Enumerations;
using System.Collections.Generic;

namespace DeliveryClub.Domain.AuxiliaryModels.Dispatcher
{
    public class GetRestaurantOrderModel
    {
        public string Name { get; set; }

        public double DeliveryCost { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public ICollection<GetOrderedProductModel> Products { get; set; }
    }
}
