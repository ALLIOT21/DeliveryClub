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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly Mapper _mapper;

        public PortionPriceManager(ApplicationDbContext dbContext,
                            UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
        }

        public ICollection<PortionPrice> CreatePortionPrices(IEnumerable<PortionPriceModel> portionPriceModels)
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
