﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DeliveryClub.Domain.AuxiliaryModels.Admin
{
    public class RestaurantInfoModel
    {
        public string Name { get; set; }

        public List<SpecializationModel> Specializations { get; set; }

        public double DeliveryCost { get; set; }

        public double MinimalOrderPrice { get; set; }

        public IFormFile CoverImage { get; set; }

        public string CoverImageName { get; set; }

        public string Description { get; set; }        

        public string DeliveryMaxTime { get; set; }

        public string OrderTimeBegin { get; set; }

        public string OrderTimeEnd { get; set; }
    }
}
