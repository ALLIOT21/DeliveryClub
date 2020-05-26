using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryClub.Web.ViewModels.Admin
{
    public class PortionPriceViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Portion { get; set; }
        
        [Required]
        public double Price { get; set; }
    }
}
