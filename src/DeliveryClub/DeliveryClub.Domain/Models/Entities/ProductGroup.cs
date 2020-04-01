using System.Collections.Generic;

namespace DeliveryClub.Domain.Models.Entities
{
    public class ProductGroup
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }

        public HashSet<int> PortionPriceIds { get; set; }

        public HashSet<PortionPrice> PortionPrices { get; set; }

        public int RestaurantId { get; set; }

        public Restaurant Restaurant { get; set; }
    }
}
