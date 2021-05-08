using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Data.DTO.EntitiesDTO
{
    public class PortionPriceProductsDAO
    {
        public int ProductId { get; set; }

        public int PortionPriceId { get; set; }

        public ProductDAO Product { get; set; }

        public PortionPriceDAO PortionPrice { get; set; }
    }
}
