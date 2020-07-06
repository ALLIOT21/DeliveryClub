using System;
using System.Collections.Generic;

namespace DeliveryClub.Data.DTO.EntitiesDTO
{
    public class RestaurantAdditionalInfoDTO
    {
        public int Id { get; set; }

        public string Description { get; set; }           

        public TimeSpan? DeliveryMaxTime { get; set; }

        public TimeSpan? OrderTimeBegin { get; set; }

        public TimeSpan? OrderTimeEnd { get; set; }

        public int RestaurantId { get; set; }

        public RestaurantDTO Restaurant { get; set; }
    }
}