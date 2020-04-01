using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Domain.Models.Entities
{
    public class OrderedProduct
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public PortionPrice PortionPrice { get; set; }

        public int Amount { get; set; }

        public Order Order { get; set; }

        public Product Product { get; set; } 
    }
}
