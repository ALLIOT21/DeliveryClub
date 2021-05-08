using DeliveryClub.Data.Context;
using DeliveryClub.Data.DTO.EntitiesDTO;
using DeliveryClub.Domain.Models.Entities;
using DeliveryClub.Infrastructure.Mapping;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DeliveryClub.Domain.Logic.Managers
{
    public class PortionPriceProductGroupManager
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Mapper _mapper;

        public PortionPriceProductGroupManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
        }
        public ICollection<PortionPriceProductGroup> CreatePortionPricesProductGroup(ICollection<PortionPrice> portionPrices, ProductGroup productGroup)
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

            var portionPricesProductGroupDTO = new List<PortionPriceProductGroupsDAO>();
            foreach (var pppg in portionPricesProductGroup)
            {
                portionPricesProductGroupDTO.Add(_mapper.Map<PortionPriceProductGroup, PortionPriceProductGroupsDAO>(pppg));
            }

            _dbContext.PortionPriceProductGroups.AddRange(portionPricesProductGroupDTO);

            var result = new List<PortionPriceProductGroup>();
            foreach (var pppg in portionPricesProductGroupDTO)
            {
                result.Add(_mapper.Map<PortionPriceProductGroupsDAO, PortionPriceProductGroup>(pppg));
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

            var portionPriceProductGroupDTO = _mapper.Map<PortionPriceProductGroup, PortionPriceProductGroupsDAO>(newPortionPriceProductGroup);

            _dbContext.PortionPriceProductGroups.Add(portionPriceProductGroupDTO);

            var result = _mapper.Map<PortionPriceProductGroupsDAO, PortionPriceProductGroup>(portionPriceProductGroupDTO);

            return result;
        }

        public void UpdatePortionPricesProductGroup(ICollection<PortionPrice> portionPrices, ProductGroup productGroup)
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

            var ppToDelete = new List<PortionPriceProductGroup>(); 
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
                {
                    ppToDelete.Add(pppg);
                    DeletePortionPriceProductGroup(pppg);
                }
            }

            foreach (var pp in ppToDelete)
            {
                productGroup.PortionPrices.Remove(pp);
            }
        }

        public ICollection<PortionPriceProductGroup> GetPortionPriceProductGroups(int productGroupId)
        {
            var pppgsDTO = _dbContext.PortionPriceProductGroups.Where(pppg => pppg.ProductGroupId == productGroupId);
            var result = new List<PortionPriceProductGroup>();
            foreach (var pppgDTO in pppgsDTO)
            {
                result.Add(_mapper.Map<PortionPriceProductGroupsDAO, PortionPriceProductGroup>(pppgDTO));
            }
            return result;
        }

        public void DeletePortionPriceProductGroup(PortionPriceProductGroup pppg)
        {
            var pppgDTO = _mapper.Map<PortionPriceProductGroup, PortionPriceProductGroupsDAO>(pppg);
            _dbContext.PortionPriceProductGroups.Remove(pppgDTO);
            _dbContext.PortionPrices.Remove(pppgDTO.PortionPrice);
        }
    }
}
