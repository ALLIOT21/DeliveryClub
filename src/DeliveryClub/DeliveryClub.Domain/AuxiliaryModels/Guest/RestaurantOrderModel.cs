using DeliveryClub.Domain.Models.Enumerations;
using System.Collections.Generic;

namespace DeliveryClub.Domain.AuxiliaryModels.Guest
{
    public class RestaurantOrderModel
    {
        public int RestaurantId { get; set; }        

        public ICollection<OrderedProductModel> Products { get; set; }
    }
}
