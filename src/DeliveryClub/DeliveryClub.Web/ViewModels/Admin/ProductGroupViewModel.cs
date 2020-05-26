using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DeliveryClub.Web.ViewModels.Admin
{
    public class ProductGroupViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<PortionPriceViewModel> PortionPrices { get; set; }
         
        public List<ProductViewModel> Products { get; set; }
    }
}
