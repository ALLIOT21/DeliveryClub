using DeliveryClub.Web.ViewModels.Admin.Info;
using System.Collections.Generic;

namespace DeliveryClub.Web.ViewModels.Guest
{
    public class RestaurantPartialViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<SpecializationViewModel> Specializations { get; set; }

        public double DeliveryCost { get; set; }

        public double MinimalOrderPrice { get; set; }
    }
}
