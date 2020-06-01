﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DeliveryClub.Web.ViewModels.Admin
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<PortionPriceViewModel> PortionPrices { get; set; }

        public string ProductGroupName { get; set; }

        public string ImageName { get; set; }

        public string ImageFolderPath { get; set; }

        public IFormFile Image { get; set; }
    }
}