using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryClub.Web.ViewModels
{
    public class UpdateAdminViewModel
    {
        public string OldEmail { get; set; }

        public string NewEmail { get; set; }

        public string Password { get; set; }
    }
}
