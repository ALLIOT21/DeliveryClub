using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Domain.Models.Entities
{
    public class PortionPrice
    {
        public int Id { get; set; }

        public string Portion { get; set; }

        public int Price { get; set; }

    }
}
