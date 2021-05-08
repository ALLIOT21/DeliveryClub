using System.Collections.Generic;

namespace DeliveryClub.Data.DTO.EntitiesDTO
{
    public class RestaurantDAO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double? MinimalOrderPrice { get; set; }

        public double? DeliveryCost { get; set; }

        public string CoverImageName { get; set; }

        public virtual HashSet<SpecializationDAO> Specializations { get; set; }

        public RestaurantAdditionalInfoDAO Info { get; set; }

        public HashSet<ProductGroupDAO> Menu { get; set; }
    }
}
