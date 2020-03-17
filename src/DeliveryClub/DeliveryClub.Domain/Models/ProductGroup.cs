using System.Collections.Generic;

namespace DeliveryClub.Domain.Models
{
    public class ProductGroup
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Product> Products { get; set; }

        public Dictionary<string, decimal> PortionPriceDictionary { get; set; } 
    }
}
