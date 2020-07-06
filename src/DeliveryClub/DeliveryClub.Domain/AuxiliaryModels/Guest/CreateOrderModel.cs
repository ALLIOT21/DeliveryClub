using DeliveryClub.Domain.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Domain.AuxiliaryModels.Guest
{
    public class CreateOrderModel
    {
        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public AddressModel DeliveryAddress { get; set; }

        public string Comment { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public ICollection<RestaurantOrderModel> Orders { get; set; }
    }
}
