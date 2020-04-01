using System;
using System.Collections.Generic;
using DeliveryClub.Domain.Models.Enumerations;

namespace DeliveryClub.Domain.Models.Entities
{
    public class RestaurantAdditionalInfo
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public double DeliveryCost { get; set; }

        public virtual HashSet<Specialization> Specializations { get; set; }

        public virtual HashSet<PaymentMethod> PaymentMethods { get; set; }

        public double MinimalOrderPrice { get; set; }

        public TimeSpan? DeliveryMaxTime { get; set; }

        public TimeSpan? OrderTimeBegin { get; set; }

        public TimeSpan? OrderTimeEnd { get; set; }

        public int RestaurantId { get; set; }

        public Restaurant Restaurant { get; set; }
    }
}
