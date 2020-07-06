using DeliveryClub.Domain.AuxiliaryModels.Admin;
using System.Collections.Generic;

namespace DeliveryClub.Domain.AuxiliaryModels.Guest
{
    public class RestaurantFullModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double DeliveryCost { get; set; }

        public double MinimalOrderPrice { get; set; }

        public List<SpecializationModel> Specializations { get; set; }

        public string Description { get; set; }

        public string DeliveryMaxTime { get; set; }

        public string OrderTimeBegin { get; set; }

        public string OrderTimeEnd { get; set; }

        public ICollection<ProductGroupModel> Menu { get; set; }
    }
}
