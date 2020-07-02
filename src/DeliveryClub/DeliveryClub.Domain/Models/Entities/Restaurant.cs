using DeliveryClub.Domain.Models.Enumerations;
using System;
using System.Collections.Generic;

namespace DeliveryClub.Domain.Models.Entities
{
    public class Restaurant
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double DeliveryCost { get; set; }

        public double MinimalOrderPrice { get; set; }

        public string CoverImageName { get; set; }

        public HashSet<Specialization> Specializations { get; set; }

        public RestaurantAdditionalInfo RestaurantAdditionalInfo { get; set; }

        public ICollection<ProductGroup> Menu { get; set; }
    }
}
