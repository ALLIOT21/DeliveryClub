using DeliveryClub.Data.DTO.EntitiesDTO;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Data.DTO.ActorsDTO
{
    public class RegisteredUserDTO
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public IdentityUser User { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
    }
}
