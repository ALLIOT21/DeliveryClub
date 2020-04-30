using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Domain.AuxiliaryModels.SuperUser
{
    public class UpdateAdminModel
    {
        public string OldEmail { get; set; }

        public string NewEmail { get; set; }

        public string Password { get; set; }
    }
}
