using System.Collections.Generic;

namespace DeliveryClub.Domain.Models.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public HashSet<OrderedProduct> OrderedProducts { get; set; }

        public int ReviewRating { get; set; }
    }
}
