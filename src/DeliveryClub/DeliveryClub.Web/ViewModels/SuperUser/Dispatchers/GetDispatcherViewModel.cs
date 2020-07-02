using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryClub.Web.ViewModels.SuperUser.Dispatchers
{
    public class GetDispatcherViewModel
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; }
    }
}
