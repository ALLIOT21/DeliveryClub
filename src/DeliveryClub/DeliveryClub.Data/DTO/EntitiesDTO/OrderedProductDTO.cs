using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Data.DTO.EntitiesDTO
{
    public class OrderedProductDTO
    {
        public int Amount { get; set; }

        public int RestaurantOrderId { get; set; }

        public int ProductId { get; set; }

        public int PortionPriceId { get; set; }

        public RestaurantOrderDTO RestaurantOrder { get; set; }

        public ProductDTO Product { get; set; }

        public PortionPriceDTO PortionPrice { get; set; }
    }
}
