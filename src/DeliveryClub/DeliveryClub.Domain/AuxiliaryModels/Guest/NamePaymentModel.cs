using DeliveryClub.Domain.AuxiliaryModels.Admin;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Domain.AuxiliaryModels.Guest
{
    public class NamePaymentModel
    {
        public string Name { get; set; }

        public List<PaymentMethodModel> PaymentMethods { get; set; }
    }
}
