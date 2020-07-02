using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Domain.Models.Entities
{
    public class OrderedProduct
    {
        public int Amount { get; set; }

        public int RestaurantOrderId { get; set; }

        public int ProductId { get; set; }

        public int PortionPriceId { get; set; }

        public RestaurantOrder RestaurantOrder { get; set; }

        public Product Product { get; set; }

        public PortionPrice PortionPrice { get; set; }
    }
}
