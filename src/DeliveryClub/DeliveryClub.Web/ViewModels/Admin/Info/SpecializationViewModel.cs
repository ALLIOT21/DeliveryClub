﻿using DeliveryClub.Domain.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryClub.Web.ViewModels.Admin.Info
{
    public class SpecializationViewModel
    {
        public Specialization Specialization { get; set; }

        public bool IsSelected { get; set; }
    }
}
