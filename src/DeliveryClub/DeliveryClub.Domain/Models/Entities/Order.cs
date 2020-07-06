using DeliveryClub.Domain.Models.Enumerations;
using System;
using System.Collections.Generic;

namespace DeliveryClub.Domain.Models.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DeliveryAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string Comment { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public DateTime DateTime { get; set; }

        public OrderStatus Status { get; set; }

        public int DispatcherId { get; set; }

        public int? RegisteredUserId { get; set; }

        public List<RestaurantOrder> RestaurantOrders { get; set; }
    }
}
