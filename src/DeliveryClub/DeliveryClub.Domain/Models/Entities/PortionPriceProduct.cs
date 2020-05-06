using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Domain.Models.Entities
{
    public class PortionPriceProduct
    {
        public int ProductId { get; set; }

        public int PortionPriceId { get; set; }

        public Product Product { get; set; }

        public PortionPrice PortionPrice { get; set; }
    }
}
