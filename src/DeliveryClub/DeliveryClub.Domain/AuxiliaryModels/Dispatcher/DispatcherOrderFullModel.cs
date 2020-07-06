using DeliveryClub.Domain.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Domain.AuxiliaryModels.Dispatcher
{
    public class DispatcherOrderFullModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DeliveryAddress { get; set; }

        public string PhoneNumber { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public string Comment { get; set; }

        public DateTime DateTime { get; set; }

        public OrderStatus Status { get; set; }

        public List<GetRestaurantOrderModel> RestaurantOrders { get; set; }

    }
}
