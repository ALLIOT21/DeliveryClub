using DeliveryClub.Domain.Models.Enumerations;
using System;
using System.Collections.Generic;

namespace DeliveryClub.Domain.Models.Entities
{
    public class Restaurant
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual HashSet<Specialization> Specializations { get; set; }

        public RestaurantAdditionalInfo AdditionalInfo { get; set; }

        public IEnumerable<ProductGroup> Menu { get; set; }

        public Review Review { get; set; }

    }
}
