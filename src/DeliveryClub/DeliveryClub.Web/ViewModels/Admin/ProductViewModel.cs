using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeliveryClub.Web.ViewModels.Admin
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }

        public List<PortionPriceViewModel> PortionPrices { get; set; }

        [Required]
        public string ProductGroupName { get; set; }

        public string ImageName { get; set; }

        public IFormFile Image { get; set; }

        public string ProductGroupPortionPriced { get; set; }
    }
}
