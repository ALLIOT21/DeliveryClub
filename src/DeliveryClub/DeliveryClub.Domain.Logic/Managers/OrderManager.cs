using DeliveryClub.Data.Context;
using DeliveryClub.Data.DTO.EntitiesDTO;
using DeliveryClub.Domain.AuxiliaryModels.Guest;
using DeliveryClub.Domain.Models.Actors;
using DeliveryClub.Domain.Models.Entities;
using DeliveryClub.Domain.Models.Enumerations;
using DeliveryClub.Infrastructure.Mapping;
using System;
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

            order = _mapper.Map<OrderDTO, Order>(_dbContext.Orders.Add(_mapper.Map<Order, OrderDTO>(order)).Entity);
            _dbContext.SaveChanges();

            foreach(var ro in model.Orders)
            {                
                _restaurantOrderManager.CreateRestaurantOrder(ro, order.Id);
            }

            await _dbContext.SaveChangesAsync();
            return order.Id;
        }

        public Order GetLastOrder()
        {
            return _mapper.Map<OrderDTO, Order>(_dbContext.Orders.OrderByDescending(p => p.Id).FirstOrDefault());
        }
    }
}
