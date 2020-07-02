using DeliveryClub.Web.ViewModels.Admin.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryClub.Web.ViewModels.Guest
{
    public class OrderedProductViewModel
    {
        public int Id { get; set; }

        public int Amount { get; set; }

        public int PortionId { get; set; }
    }
}
