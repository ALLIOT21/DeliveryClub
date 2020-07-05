using DeliveryClub.Domain.Models.Enumerations;
using System;
using System.Collections.Generic;

namespace DeliveryClub.Web.ViewModels.Dispatchers
{
    public class DispatcherOrderFullViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DeliveryAddress { get; set; }

        public string PhoneNumber { get; set; }

        public string Comment { get; set; }

        public DateTime DateTime { get; set; }

        public OrderStatus Status { get; set; }

        public List<GetRestaurantOrderViewModel> RestaurantOrders { get; set; }
    }
}
