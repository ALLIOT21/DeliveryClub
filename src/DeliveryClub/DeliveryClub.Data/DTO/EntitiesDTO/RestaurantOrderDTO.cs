using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Data.DTO.EntitiesDTO
{
    public class RestaurantOrderDTO
    {
        public int Id { get; set; }

        public PaymentMethodDTO PaymentMethod { get; set; }

        public List<OrderedProductDTO> OrderedProducts { get; set; }

        public OrderDTO Order { get; set; }

        public int OrderId { get; set; }
    }
}
