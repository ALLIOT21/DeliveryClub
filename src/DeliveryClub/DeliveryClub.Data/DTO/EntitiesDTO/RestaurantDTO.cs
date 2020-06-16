using System.Collections.Generic;

namespace DeliveryClub.Data.DTO.EntitiesDTO
{
    public class RestaurantDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double? MinimalOrderPrice { get; set; }

        public double? DeliveryCost { get; set; }

        public string CoverImageName { get; set; }

        public virtual HashSet<SpecializationDTO> Specializations { get; set; }

        public RestaurantAdditionalInfoDTO Info { get; set; }

        public HashSet<ProductGroupDTO> Menu { get; set; }

        public ReviewDTO Review { get; set; }
    }
}
