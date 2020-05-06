using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Domain.Models.Entities
{
    public class PortionPriceProductGroup
    {
        public int ProductGroupId { get; set; }

        public int PortionPriceId { get; set; }

        public ProductGroup ProductGroup { get; set; }

        public PortionPrice PortionPrice { get; set; }
    }
}
