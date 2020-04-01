using System;
using System.Collections.Generic;
using DeliveryClub.Data.DTO.Enumerations;

namespace DeliveryClub.Data.DTO.EntitiesDTO
{
    public class RestaurantAdditionalInfoDTO
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public double DeliveryCost { get; set; }

        public virtual HashSet<PaymentMethodDTO> PaymentMethods { get; set; }

        public double MinimalOrderPrice { get; set; }

        public TimeSpan? DeliveryMaxTime { get; set; }

        public TimeSpan? OrderTimeBegin { get; set; }

        public TimeSpan? OrderTimeEnd { get; set; }
    }
}
