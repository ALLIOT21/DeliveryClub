using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Data.DTO.EntitiesDTO
{
    public class PortionPriceProductGroupsDTO
    {   
        public int ProductGroupId { get; set; }

        public int PortionPriceId { get; set; }      

        public ProductGroupDTO ProductGroup { get; set; }
        
        public PortionPriceDTO PortionPrice { get; set; }
    }
}
