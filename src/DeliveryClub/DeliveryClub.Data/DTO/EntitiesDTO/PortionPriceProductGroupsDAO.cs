using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Data.DTO.EntitiesDTO
{
    public class PortionPriceProductGroupsDAO
    {   
        public int ProductGroupId { get; set; }

        public int PortionPriceId { get; set; }      

        public ProductGroupDAO ProductGroup { get; set; }
        
        public PortionPriceDAO PortionPrice { get; set; }
    }
}
