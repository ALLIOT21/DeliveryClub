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
    public class RestaurantOrderManager
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly OrderedProductManager _orderedProductManager;
        private readonly Mapper _mapper;

        public RestaurantOrderManager(ApplicationDbContext dbContext,
                                      OrderedProductManager orderedProductManager)
        {
            _dbContext = dbContext;
            _orderedProductManager = orderedProductManager;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
        }

        public RestaurantOrder CreateRestaurantOrder(RestaurantOrderModel model, int id)
        {
            var restaurantOrder = new RestaurantOrder
            {
                PaymentMethod = model.PaymentMethod,
                OrderedProducts = new List<OrderedProduct>(),
                OrderId = id,
                RestaurantId = model.RestaurantId,                
            };

            var ops = new List<OrderedProduct>();
            foreach (var op in model.Products)
            {
                ops.Add(_orderedProductManager.CreateOrderedProduct(op));
            }

            restaurantOrder.OrderedProducts = ops;

            return _mapper.Map<RestaurantOrderDTO, RestaurantOrder>(_dbContext.RestaurantOrders.Add(_mapper.Map<RestaurantOrder, RestaurantOrderDTO>(restaurantOrder)).Entity);
        }

        public ICollection<RestaurantOrder> GetRestaurantOrders(int orderId)
        {
            var rosDTO = _dbContext.RestaurantOrders.Where(ro => ro.OrderId == orderId).Include(ro => ro.Restaurant).ToList();

            var result = new List<RestaurantOrder>();
            foreach(var rodto in rosDTO)
            {
                var ro = _mapper.Map<RestaurantOrderDTO, RestaurantOrder>(rodto);
                result.Add(ro); 
            }

            return result;
        }
    }
}
