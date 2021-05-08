using DeliveryClub.Data.Context;
using DeliveryClub.Data.DTO.EntitiesDTO;
using DeliveryClub.Domain.AuxiliaryModels.Guest;
using DeliveryClub.Domain.Models.Entities;
using DeliveryClub.Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DeliveryClub.Domain.Logic.Managers
{
    public class OrderedProductManager
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Mapper _mapper;

        public OrderedProductManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
        }

        public OrderedProduct CreateOrderedProduct(OrderedProductModel model)
        {
            var op = new OrderedProduct
            {
                Amount = model.Amount,
                ProductId = model.Id,
                PortionPriceId = model.PortionId,
            };
            return op;
        }

        public ICollection<OrderedProduct> GetOrderedProducts(int restaurantOrderId)
        {
            var opsDTO = _dbContext.OrderedProducts.Where(op => op.RestaurantOrderId == restaurantOrderId).Include(op => op.PortionPrice).Include(op => op.Product);

            var result = new List<OrderedProduct>();
            foreach(var opDTO in opsDTO)
            {
                var op = _mapper.Map<OrderedProductDAO, OrderedProduct>(opDTO);
                result.Add(op);
            }

            return result;
        }
    }
}
