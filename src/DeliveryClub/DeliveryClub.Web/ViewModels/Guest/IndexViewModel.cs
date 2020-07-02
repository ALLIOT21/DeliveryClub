using System.Collections.Generic;

namespace DeliveryClub.Web.ViewModels.Guest
{
    public class IndexViewModel
    {
        public PageViewModel PageViewModel { get; set; }

        public ICollection<RestaurantPartialViewModel> RestaurantPartialViewModels { get; set; }
    }
}
