using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryClub.Web.ViewModels.SuperUser.Dispatchers
{
    public class UpdateDispatcherViewModel
    {        
        public int Id { get; set; }

        public string OldEmail { get; set; }

        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",
            ErrorMessage = "E-mail is incorrect.")]
        public string NewEmail { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
