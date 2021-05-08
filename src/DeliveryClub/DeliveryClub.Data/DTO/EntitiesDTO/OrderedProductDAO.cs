using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Data.DTO.EntitiesDTO
{
    public class OrderedProductDAO
    {
        public int Amount { get; set; }

        public int RestaurantOrderId { get; set; }

        public int ProductId { get; set; }

        public int PortionPriceId { get; set; }

        public RestaurantOrderDAO RestaurantOrder { get; set; }

        public ProductDAO Product { get; set; }

        public PortionPriceDAO PortionPrice { get; set; }
    }
}
