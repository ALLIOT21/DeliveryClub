using DeliveryClub.Domain.AuxiliaryModels.Admin;
using System.Collections.Generic;

namespace DeliveryClub.Domain.AuxiliaryModels.Guest
{
    public class RestaurantPartialModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<SpecializationModel> Specializations { get; set; }

        public double DeliveryCost { get; set; }

        public double MinimalOrderPrice { get; set; }

        public string CoverImageName { get; set; }
    }
}
