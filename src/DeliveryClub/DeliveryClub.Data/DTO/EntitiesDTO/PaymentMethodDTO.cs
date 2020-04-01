using System;
using System.Collections.Generic;
using System.Text;
using DeliveryClub.Data.DTO.Enumerations;

namespace DeliveryClub.Data.DTO.EntitiesDTO
{
    public class PaymentMethodDTO
    {
        public int Id { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
    }
}
