using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Data.DTO.EntitiesDTO
{
    public class ProductGroupDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public HashSet<ProductDTO> Products { get; set; }

        public HashSet<PortionPriceDTO> PortionPrices { get; set; }

        public int RestaurantId { get; set; }

        public RestaurantDTO Restaurant { get; set; }
    }
}
