using System.Collections.Generic;

namespace DeliveryClub.Domain.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageName { get; set; }

        public HashSet<PortionPriceProduct> PortionPrices { get; set; }

        public int ProductGroupId { get; set; }

        public ProductGroup ProductGroup { get; set; }
    }
}
