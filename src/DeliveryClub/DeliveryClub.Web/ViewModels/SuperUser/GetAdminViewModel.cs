﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryClub.Web.ViewModels.SuperUser
{
    public class GetAdminViewModel
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string RestaurantName { get; set; }
    }
}
