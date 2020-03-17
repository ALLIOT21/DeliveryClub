using DeliveryClub.Domain.Enumerations;
using System;
using System.Collections.Generic;

namespace DeliveryClub.Domain.Models
{
    public class Restaurant
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<ProductGroup> Menu { get; set; }

        public List<Specialization> Specializations { get; set; }

        public List<PaymentMethod> PaymentMethods { get; set; }

        public decimal DeliveryCost { get; set; }

        public decimal MinimalOrderPrice { get; set; }

        public TimeSpan DeliveryTime { get; set; }

        public TimeSpan OrderTimeBegin { get; set; }

        public TimeSpan OrderTimeEnd { get; set; }

        public int ReviewAmount { get; set; }

        public double ReviewOverallRating { get; set; }
    }
}
