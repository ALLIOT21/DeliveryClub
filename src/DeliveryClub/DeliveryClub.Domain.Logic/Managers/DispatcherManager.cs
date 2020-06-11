using DeliveryClub.Data.Context;
using DeliveryClub.Data.DTO.ActorsDTO;
using DeliveryClub.Domain.AuxiliaryModels.Admin;
using DeliveryClub.Domain.Models.Actors;
using DeliveryClub.Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DeliveryClub.Domain.Logic.Managers
{
    public class DispatcherManager
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Mapper _mapper;

        public DispatcherManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
        }

        public ICollection<Dispatcher> GetDispatchers(int restaurantId)
        {
            var dispatcherDtos = _dbContext.Dispatchers.Where(d => d.RestaurantId == restaurantId).Include(d => d.User).ToList();

            var dispatchers = new List<Dispatcher>();
            foreach (var d in dispatcherDtos)
            {
                dispatchers.Add(_mapper.Map<DispatcherDTO, Dispatcher>(d));
            }

            return dispatchers;
        }

        public async Task<Dispatcher> CreateDispatcher(string userId, int restaurantId)
        {
            var d = new Dispatcher()
            {
                RestaurantId = restaurantId,
                UserId = userId,
            };
            var result = await _dbContext.Dispatchers.AddAsync(_mapper.Map<Dispatcher, DispatcherDTO>(d));
            return _mapper.Map<DispatcherDTO, Dispatcher>(result.Entity);
        }
    }
}