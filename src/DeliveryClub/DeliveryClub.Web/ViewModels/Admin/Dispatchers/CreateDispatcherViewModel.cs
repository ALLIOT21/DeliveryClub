using System.ComponentModel.DataAnnotations;

namespace DeliveryClub.Web.ViewModels.Admin.Dispatchers
{
    public class CreateDispatcherViewModel
    {
        [Required]
        [RegularExpression(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$",
            ErrorMessage = "E-mail is incorrect.")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
