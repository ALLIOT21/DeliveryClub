using DeliveryClub.Web.ViewModels.Admin.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryClub.Web.ViewModels.Guest
{
    public class NamePaymentViewModel
    {
        public string Name { get; set; }

        public List<PaymentMethodViewModel> PaymentMethods { get; set; }
     }
}
