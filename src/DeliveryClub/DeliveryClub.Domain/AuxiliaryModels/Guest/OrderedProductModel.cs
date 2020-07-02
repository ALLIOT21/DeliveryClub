using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Domain.AuxiliaryModels.Guest
{
    public class OrderedProductModel
    {
        public int Id { get; set; }

        public int Amount { get; set; }

        public int PortionId { get; set; }
    }
}
