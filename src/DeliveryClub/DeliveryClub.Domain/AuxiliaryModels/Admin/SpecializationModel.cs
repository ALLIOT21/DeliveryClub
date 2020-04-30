using DeliveryClub.Domain.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Domain.AuxiliaryModels.Admin
{
    public class SpecializationModel
    {
        public Specialization Specialization { get; set; }

        public bool IsSelected { get; set; }
    }
}
