using DeliveryClub.Data.Context;
using DeliveryClub.Domain.AuxiliaryModels.Admin;
using DeliveryClub.Domain.Models.Entities;
using DeliveryClub.Infrastructure.Mapping;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DeliveryClub.Domain.Logic.Managers
{
    public class PortionPriceManager
    {
        public ICollection<PortionPrice> CreatePortionPrices(ICollection<PortionPriceModel> portionPriceModels)
        {
            if (portionPriceModels != null)
            {
                var portionPrices = new List<PortionPrice>();
                foreach (var pp in portionPriceModels)
                {
                    var portionPrice = new PortionPrice
                    {
                        Id = pp.Id,
                        Portion = pp.Portion,
                        Price = pp.Price
                    };
                    portionPrices.Add(portionPrice);
                }
                return portionPrices;
            }
            return null;
        }
    }
}
