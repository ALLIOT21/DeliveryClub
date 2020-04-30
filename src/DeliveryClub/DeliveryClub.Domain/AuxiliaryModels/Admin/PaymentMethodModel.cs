using DeliveryClub.Domain.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Domain.AuxiliaryModels.Admin
{
    public class PaymentMethodModel
    {
        public PaymentMethod PaymentMethod { get; set; }

        public bool IsSelected { get; set; }
    }
}
