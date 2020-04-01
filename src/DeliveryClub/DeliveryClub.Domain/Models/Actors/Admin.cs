using DeliveryClub.Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace DeliveryClub.Domain.Models.Actors
{ 
    public class Admin : IdentityUser 
    {
        public int RestaurantId { get; set; }

        public Restaurant Restaurant { get; set; }

    }
}
