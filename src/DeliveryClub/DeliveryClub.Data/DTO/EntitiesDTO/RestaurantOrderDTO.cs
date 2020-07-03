﻿using DeliveryClub.Domain.Models.Enumerations;
using System.Collections.Generic;

namespace DeliveryClub.Data.DTO.EntitiesDTO
{
    public class RestaurantOrderDTO
    {
        public int Id { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public List<OrderedProductDTO> OrderedProducts { get; set; }

        public OrderDTO Order { get; set; }

        public int OrderId { get; set; }
    }
}