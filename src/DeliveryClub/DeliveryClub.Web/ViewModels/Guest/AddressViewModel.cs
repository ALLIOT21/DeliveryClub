using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace DeliveryClub.Web.ViewModels.Guest
{
    public class AddressViewModel
    {
        [Required]
        public string Street { get; set; }

        [Required(ErrorMessage = "Please enter a house number")]
        [Range(1, int.MaxValue, ErrorMessage = "House value must be positive")]
        public int House { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Building value must be positive")]
        public int? Building { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Flat value must be positive")]
        public int? Flat { get; set; }
    }
}
