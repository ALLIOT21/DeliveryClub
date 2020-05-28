using System.Collections.Generic;

namespace DeliveryClub.Data.DTO.EntitiesDTO
{
    public class ProductDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public HashSet<PortionPriceProductsDTO> PortionPrices { get; set; }

        public int ProductGroupId { get; set; }

        public ProductGroupDTO ProductGroup { get; set; }
    }
}
