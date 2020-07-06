using DeliveryClub.Domain.Models.Enumerations;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace DeliveryClub.Web.ViewModels.Admin.Info
{
    public class RestaurantInfoViewModel
    {
        public string Name { get; set; }

        public List<SpecializationViewModel> Specializations { get; set; }

        public double DeliveryCost { get; set; }

        public double MinimalOrderPrice { get; set; }

        public string Description { get; set; }

        public IFormFile CoverImage { get; set; }

        public string CoverImageName { get; set; }

        public string DeliveryMaxTime { get; set; }

        public string OrderTimeBegin { get; set; }

        public string OrderTimeEnd { get; set; } 
    }
}
