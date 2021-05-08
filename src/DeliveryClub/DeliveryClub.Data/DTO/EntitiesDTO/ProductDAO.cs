using System.Collections.Generic;

namespace DeliveryClub.Data.DTO.EntitiesDTO
{
    public class ProductDAO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageName { get; set; }

        public HashSet<PortionPriceProductsDAO> PortionPrices { get; set; }

        public int ProductGroupId { get; set; }

        public ProductGroupDAO ProductGroup { get; set; }
    }
}
