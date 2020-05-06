using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Domain.AuxiliaryModels.Admin
{
    public class ProductGroupModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<PortionPriceModel> PortionPrices { get; set; }
    }
}
