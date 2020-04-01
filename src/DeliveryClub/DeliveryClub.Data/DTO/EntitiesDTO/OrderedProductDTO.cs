using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Data.DTO.EntitiesDTO
{
    public class OrderedProductDTO
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int PortionPriceId { get; set; } 

        public PortionPriceDTO PortionPrice { get; set; }

        public int Amount { get; set; }

        public OrderDTO Order { get; set; }

        public ProductDTO Product { get; set; }
    }
}
