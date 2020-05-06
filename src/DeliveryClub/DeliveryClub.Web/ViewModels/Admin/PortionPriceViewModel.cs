using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryClub.Web.ViewModels.Admin
{
    public class PortionPriceViewModel
    {
        public int Id { get; set; }

        public string Portion { get; set; }
        
        public double Price { get; set; }
    }
}
