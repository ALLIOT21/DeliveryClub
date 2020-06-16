using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryClub.Web.ViewModels.Guest
{
    public class IndexViewModel
    {
        public PageViewModel PageViewModel { get; set; }

        public ICollection<RestaurantPartialViewModel> RestaurantPartialViewModels { get; set; }
    }
}
