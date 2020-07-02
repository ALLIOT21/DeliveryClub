using DeliveryClub.Domain.Models.Enumerations;
using System.Collections.Generic;

namespace DeliveryClub.Domain.Models.Entities
{
    public class RestaurantOrder
    {
        public int Id { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public List<OrderedProduct> OrderedProducts { get; set; }

        public Order Order { get; set; }

        public int OrderId { get; set; }        
    }
}
