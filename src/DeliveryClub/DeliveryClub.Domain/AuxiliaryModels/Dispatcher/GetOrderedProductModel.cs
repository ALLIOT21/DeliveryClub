using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Domain.AuxiliaryModels.Dispatcher
{
    public class GetOrderedProductModel
    {
        public string Name { get; set; }

        public int Amount { get; set; }

        public string Portion { get; set; }

        public double Price { get; set; }
    }
}
