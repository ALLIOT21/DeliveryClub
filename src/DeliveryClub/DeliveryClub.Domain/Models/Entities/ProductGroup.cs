using System.Collections.Generic;

namespace DeliveryClub.Domain.Models.Entities
{
    public class ProductGroup
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public HashSet<Product> Products { get; set; }

        public HashSet<PortionPriceProductGroup> PortionPrices { get; set; }

        public int RestaurantId { get; set; }

        public Restaurant Restaurant { get; set; }
    }
}
