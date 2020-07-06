using DeliveryClub.Web.ViewModels.Admin.Info;
using DeliveryClub.Web.ViewModels.Admin.Menu;
using System.Collections.Generic;

namespace DeliveryClub.Web.ViewModels.Guest
{
    public class RestaurantFullViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double DeliveryCost { get; set; }

        public double MinimalOrderPrice { get; set; }

        public List<SpecializationViewModel> Specializations { get; set; }

        public string Description { get; set; }

        public string DeliveryMaxTime { get; set; }

        public string OrderTimeBegin { get; set; }

        public string OrderTimeEnd { get; set; }

        public ICollection<ProductGroupViewModel> Menu { get; set; }
    }
}
