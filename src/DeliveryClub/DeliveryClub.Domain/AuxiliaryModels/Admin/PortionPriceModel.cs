using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Domain.AuxiliaryModels.Admin
{
    public class PortionPriceModel
    {
        public int Id { get; set; }

        public string Portion { get; set; }

        public double Price { get; set; }
    }
}
