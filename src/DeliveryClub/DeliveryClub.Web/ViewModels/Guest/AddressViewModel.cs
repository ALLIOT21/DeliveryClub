using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryClub.Web.ViewModels.Guest
{
    public class AddressViewModel
    {
        public string Street { get; set; }

        public int House { get; set; }

        public int Building { get; set; }

        public int Flat { get; set; }
    }
}
