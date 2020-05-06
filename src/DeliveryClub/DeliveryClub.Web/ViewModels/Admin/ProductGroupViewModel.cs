using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryClub.Web.ViewModels.Admin
{
    public class ProductGroupViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<PortionPriceViewModel> PortionPrices { get; set; }
    }
}
