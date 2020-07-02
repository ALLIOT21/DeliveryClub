using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Domain.AuxiliaryModels.SuperUser
{
    public class GetDispatcherModel
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }
    }
}
