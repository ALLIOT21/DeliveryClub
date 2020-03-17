using System.Collections.Generic;

namespace DeliveryClub.Domain.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Dictionary<string, decimal> PortionPriceDictionary { get; set; }
    }
}
