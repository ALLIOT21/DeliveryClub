using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Data.DTO.EntitiesDTO
{
    public class ProductGroupDAO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public HashSet<ProductDAO> Products { get; set; }

        public HashSet<PortionPriceProductGroupsDAO> PortionPrices { get; set; }

        public int RestaurantId { get; set; }

        public RestaurantDAO Restaurant { get; set; }
    }
}
