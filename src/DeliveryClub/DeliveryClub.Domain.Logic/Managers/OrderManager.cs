using DeliveryClub.Data.Context;
using DeliveryClub.Data.DTO.EntitiesDTO;
using DeliveryClub.Domain.AuxiliaryModels.Guest;
using DeliveryClub.Domain.Models.Actors;
using DeliveryClub.Domain.Models.Entities;
using DeliveryClub.Domain.Models.Enumerations;
using DeliveryClub.Infrastructure.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DeliveryClub.Domain.Logic.Managers
{
    public class OrderManager
    {
        private readonly RestaurantOrderManager _restaurantOrderManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly Mapper _mapper;
        private readonly DispatcherManager _dispatcherManager;

        public OrderManager(RestaurantOrderManager restaurantOrderManager,
                            ApplicationDbContext dbContext, 
                            DispatcherManager dispatcherManager)
        {
            _restaurantOrderManager = restaurantOrderManager;
            _dbContext = dbContext;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
            _dispatcherManager = dispatcherManager;
        }

        public ICollection<Order> GetOrders(OrderStatus orderStatus, int dispId)
        {
            var orders = new List<Order>();
            foreach (var o in _dbContext.Orders.Where(o => o.DispatcherId == dispId).Where(o => o.Status == orderStatus).ToList())
            {
                orders.Add(_mapper.Map<OrderDTO, Order>(o));
            };
            return orders;
        }

        public async Task<int> CreateOrder(CreateOrderModel model)
        {
            var lastOrder = GetLastOrder();
            int dispatcherId = lastOrder != null ? lastOrder.DispatcherId : 0;            
            var nextDispatcherId = _dispatcherManager.GetNextActiveDispatcher(dispatcherId).Id;               

            var order = new Order
            {
                Name = model.Name,
                DeliveryAddress = $"str. {model.DeliveryAddress.Street}, build. {model.DeliveryAddress.Building}, h. {model.DeliveryAddress.House}, fl. {model.DeliveryAddress.Flat}",
                PhoneNumber = model.PhoneNumber,
                Comment = model.Comment,
                Status = OrderStatus.Received,
                DateTime = DateTime.Now,
                DispatcherId = nextDispatcherId,
            };

            var newOrder = _dbContext.Orders.Add(_mapper.Map<Order, OrderDTO>(order)).Entity;
            _dbContext.SaveChanges();

            foreach(var ro in model.Orders)
            {                
                _restaurantOrderManager.CreateRestaurantOrder(ro, newOrder.Id);
            }

            await _dbContext.SaveChangesAsync();
            return newOrder.Id;
        }

        public Order GetLastOrder()
        {
            return _mapper.Map<OrderDTO, Order>(_dbContext.Orders.OrderByDescending(p => p.Id).FirstOrDefault());
        }
    }
}
