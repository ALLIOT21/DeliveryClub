using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Data.DTO.EntitiesDTO
{
    public class PortionPriceProductsDTO
    {
        public int ProductId { get; set; }

        public int PortionPriceId { get; set; }

        public ProductDTO Product { get; set; }

        public PortionPriceDTO PortionPrice { get; set; }
    }
}
