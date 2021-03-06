﻿using DeliveryClub.Domain.Models.Enumerations;
using System;
using System.Collections.Generic;

namespace DeliveryClub.Data.DTO.EntitiesDTO
{
    public class OrderDAO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DeliveryAddress { get; set; }

        public string PhoneNumber { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public string Comment { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime DateTime { get; set; }

        public int DispatcherId { get; set; }

        public int? RegisteredUserId { get; set; }

        public List<RestaurantOrderDAO> RestaurantOrders { get; set; }
    }
}
