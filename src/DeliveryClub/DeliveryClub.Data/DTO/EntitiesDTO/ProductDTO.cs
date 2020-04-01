using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Data.DTO.EntitiesDTO
{
    public class ProductDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public HashSet<PortionPriceDTO> PortionPrices { get; set; }

        public int ProductGroupId { get; set; }

        public ProductGroupDTO ProductGroup { get; set; }
    }
}
