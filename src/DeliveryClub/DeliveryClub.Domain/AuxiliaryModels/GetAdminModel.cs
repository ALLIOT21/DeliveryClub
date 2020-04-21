using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Domain.AuxiliaryModels
{
    public class GetAdminModel
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string RestaurantName { get; set; }
    }
}
