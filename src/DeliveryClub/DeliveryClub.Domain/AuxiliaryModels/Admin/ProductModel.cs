using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Domain.AuxiliaryModels.Admin
{
    public class ProductModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<PortionPriceModel> PortionPrices { get; set; }

        public string ProductGroupName { get; set; }

        public string Image { get; set; }
    }
}
