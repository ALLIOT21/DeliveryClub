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
        private readonly OrderedProductManager _orderedProductManager;

        public OrderManager(RestaurantOrderManager restaurantOrderManager,
                            ApplicationDbContext dbContext, 
                            DispatcherManager dispatcherManager,
                            OrderedProductManager orderedProductManager)
        {
            _restaurantOrderManager = restaurantOrderManager;
            _dbContext = dbContext;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
            _dispatcherManager = dispatcherManager;
            _orderedProductManager = orderedProductManager;
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

        public Order GetOrder(int id)
        {
            return _mapper.Map<OrderDTO, Order>(_dbContext.Orders.Where(o => o.Id == id).FirstOrDefault());
        }

        public Order GetFullOrder(int id)
        {
            var order = GetOrder(id);

            order.RestaurantOrders = _restaurantOrderManager.GetRestaurantOrders(order.Id).ToList();

            foreach(var ro in order.RestaurantOrders)
            {
                ro.OrderedProducts = _orderedProductManager.GetOrderedProducts(ro.Id).ToList();
            }

            return order;
        }

        public async Task SetOrderStatus(Order order, OrderStatus orderStatus)
        {
            order.Status = orderStatus;
            _dbContext.Orders.Update(_mapper.Map<Order, OrderDTO>(order));
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Order> CreateOrder(CreateOrderModel model)
        {
            var lastOrder = GetLastOrder();
            int dispatcherId = lastOrder != null ? lastOrder.DispatcherId : 0;            
            var nextDispatcherId = _dispatcherManager.GetNextActiveDispatcher(dispatcherId).Id;

            var building = model.DeliveryAddress.Building != 0 ? $", b. {model.DeliveryAddress.Building}" : "";
            var flat = model.DeliveryAddress.Flat != 0 ? $", fl. {model.DeliveryAddress.Flat}" : "";
            var deliveryAddress = $"str. {model.DeliveryAddress.Street}, h. {model.DeliveryAddress.House}" + building + flat;

            var order = new Order
            {
                Name = model.Name,
                DeliveryAddress = deliveryAddress,
                PhoneNumber = model.PhoneNumber,
                Comment = model.Comment,
                PaymentMethod = model.PaymentMethod,
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
            return _mapper.Map<OrderDTO, Order>(newOrder);
        }

        public Order GetLastOrder()
        {
            return _mapper.Map<OrderDTO, Order>(_dbContext.Orders.OrderByDescending(p => p.Id).FirstOrDefault());
        }
    }
}
