using DeliveryClub.Domain.Models.Enumerations;
using System.Collections.Generic;

namespace DeliveryClub.Data.DTO.EntitiesDTO
{
    public class RestaurantOrderDAO
    {
        public int Id { get; set; }

        public List<OrderedProductDAO> OrderedProducts { get; set; }

        public OrderDAO Order { get; set; }

        public int OrderId { get; set; }

        public RestaurantDAO Restaurant { get; set; }

        public int RestaurantId { get; set; }
    }
}
