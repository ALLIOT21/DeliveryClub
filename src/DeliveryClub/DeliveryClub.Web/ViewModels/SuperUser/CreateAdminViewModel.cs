using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryClub.Web.ViewModels.SuperUser
{
    public class CreateAdminViewModel
    {
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",
            ErrorMessage ="E-mail is incorrect.")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
