using DeliveryClub.Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Domain.Models.Actors
{
    public class Dispatcher
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public IdentityUser User { get; set; }
    }
}
