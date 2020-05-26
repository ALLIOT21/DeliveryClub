using DeliveryClub.Data.Context;
using DeliveryClub.Data.DTO.EntitiesDTO;
using DeliveryClub.Domain.Models.Entities;
using DeliveryClub.Infrastructure.Mapping;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DeliveryClub.Domain.Logic.Managers
{
    public class PortionPriceProductGroupManager
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly Mapper _mapper;

        public PortionPriceProductGroupManager(ApplicationDbContext dbContext,
                            UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
        }
        public ICollection<PortionPriceProductGroup> CreatePortionPricesProductGroup(IEnumerable<PortionPrice> portionPrices, ProductGroup productGroup)
        {
            var portionPricesProductGroup = new List<PortionPriceProductGroup>();
            foreach (var pp in portionPrices)
            {
                var newPortionPriceProductGroup = new PortionPriceProductGroup
                {
                    PortionPriceId = pp.Id,
                    PortionPrice = pp,
                    ProductGroupId = productGroup.Id,
                };
                portionPricesProductGroup.Add(newPortionPriceProductGroup);
            }

            var portionPricesProductGroupDTO = new List<PortionPriceProductGroupsDTO>();
            foreach (var pppg in portionPricesProductGroup)
            {
                portionPricesProductGroupDTO.Add(_mapper.Map<PortionPriceProductGroup, PortionPriceProductGroupsDTO>(pppg));
            }

            _dbContext.PortionPriceProductGroups.AddRange(portionPricesProductGroupDTO);

            var result = new List<PortionPriceProductGroup>();
            foreach (var pppg in portionPricesProductGroupDTO)
            {
                result.Add(_mapper.Map<PortionPriceProductGroupsDTO, PortionPriceProductGroup>(pppg));
            }

            return result;
        }

        public PortionPriceProductGroup CreatePortionPriceProductGroup(PortionPrice portionPrice, ProductGroup productGroup)
        {
            var newPortionPriceProductGroup = new PortionPriceProductGroup
            {
                PortionPriceId = portionPrice.Id,
                PortionPrice = portionPrice,
                ProductGroupId = productGroup.Id,
            };

            var portionPriceProductGroupDTO = _mapper.Map<PortionPriceProductGroup, PortionPriceProductGroupsDTO>(newPortionPriceProductGroup);

            _dbContext.PortionPriceProductGroups.Add(portionPriceProductGroupDTO);

            var result = _mapper.Map<PortionPriceProductGroupsDTO, PortionPriceProductGroup>(portionPriceProductGroupDTO);

            return result;
        }

        public void UpdatePortionPricesProductGroup(IEnumerable<PortionPrice> portionPrices, ProductGroup productGroup)
        {
            foreach (var pp in portionPrices)
            {
                bool newPppg = true;
                foreach (var pppg in productGroup.PortionPrices)
                {
                    if (pp.Id == pppg.PortionPrice.Id)
                    {
                        pppg.PortionPrice.Portion = pp.Portion;
                        pppg.PortionPrice.Price = pp.Price;
                        newPppg = false;
                    }
                }
                if (newPppg)
                    CreatePortionPriceProductGroup(pp, productGroup);
            }

            foreach (var pppg in productGroup.PortionPrices)
            {
                bool toDelete = true;
                foreach (var pp in portionPrices)
                {
                    if (pppg.PortionPrice.Id == pp.Id)
                    {
                        toDelete = false;
                    }
                }
                if (toDelete)
                    DeletePortionPriceProductGroup(pppg);
            }
        }

        public void DeletePortionPriceProductGroup(PortionPriceProductGroup pppg)
        {
            var pppgDTO = _mapper.Map<PortionPriceProductGroup, PortionPriceProductGroupsDTO>(pppg);
            _dbContext.PortionPriceProductGroups.Remove(pppgDTO);
            _dbContext.PortionPrices.Remove(pppgDTO.PortionPrice);
        }
    }
}
