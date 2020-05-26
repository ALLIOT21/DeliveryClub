using DeliveryClub.Data.Context;
using DeliveryClub.Data.DTO.EntitiesDTO;
using DeliveryClub.Domain.Models.Entities;
using DeliveryClub.Infrastructure.Mapping;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DeliveryClub.Domain.Logic.Managers
{
    public class PortionPriceProductManager
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly Mapper _mapper;

        public PortionPriceProductManager(ApplicationDbContext dbContext,
                            UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
        }

        public ICollection<PortionPriceProduct> CreatePortionPricesProduct(IEnumerable<PortionPrice> portionPrices, Product product)
        {
            var portionPricesProduct = new List<PortionPriceProduct>();
            foreach (var pp in portionPrices)
            {
                var newPortionPriceProduct = new PortionPriceProduct
                {
                    PortionPriceId = pp.Id,
                    PortionPrice = pp,
                    ProductId = product.Id
                };
                portionPricesProduct.Add(newPortionPriceProduct);
            }

            var portionPricesProductDTO = new List<PortionPriceProductsDTO>();
            foreach (var ppp in portionPricesProduct)
            {
                portionPricesProductDTO.Add(_mapper.Map<PortionPriceProduct, PortionPriceProductsDTO>(ppp));
            }

            _dbContext.PortionPriceProducts.AddRange(portionPricesProductDTO);
            _dbContext.SaveChanges();

            var result = new List<PortionPriceProduct>();
            foreach (var ppp in portionPricesProductDTO)
            {
                result.Add(new PortionPriceProduct
                {
                    PortionPrice = _mapper.Map<PortionPriceDTO, PortionPrice>(ppp.PortionPrice)
                });
            }

            return result;
        }

        public PortionPriceProduct CreatePortionPriceProduct(PortionPrice portionPrice, Product product)
        {
            var newPortionPriceProduct = new PortionPriceProduct
            {
                PortionPriceId = portionPrice.Id,
                PortionPrice = portionPrice,
                ProductId = product.Id,
            };

            var portionPriceProductDTO = _mapper.Map<PortionPriceProduct, PortionPriceProductsDTO>(newPortionPriceProduct);

            _dbContext.PortionPriceProducts.Add(portionPriceProductDTO);

            var result = _mapper.Map<PortionPriceProductsDTO, PortionPriceProduct>(portionPriceProductDTO);

            return result;
        }

        public ICollection<PortionPriceProduct> GetPortionPriceProducts(int productId)
        {
            var portionPriceProductsDTO = _dbContext.PortionPriceProducts.Where(ppp => ppp.ProductId == productId)
                                                                         .Include(ppp => ppp.PortionPrice);
            var result = new List<PortionPriceProduct>();
            foreach (var pppdto in portionPriceProductsDTO)
            {
                result.Add(_mapper.Map<PortionPriceProductsDTO, PortionPriceProduct>(pppdto));
            }
            return result;
        }
    }
}
