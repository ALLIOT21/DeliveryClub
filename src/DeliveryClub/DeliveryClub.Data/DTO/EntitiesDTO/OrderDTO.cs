using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Data.DTO.EntitiesDTO
{
    public class OrderDTO
    {
        public int Id { get; set; }

        public HashSet<OrderedProductDTO> OrderedProducts { get; set; }

        public int ReviewRating { get; set; }

    }
}
