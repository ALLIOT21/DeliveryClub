using DeliveryClub.Data.Context;
using DeliveryClub.Data.DTO.ActorsDTO;
using DeliveryClub.Domain.AuxiliaryModels.Admin;
using DeliveryClub.Domain.Models.Actors;
using DeliveryClub.Infrastructure.Mapping;
using Microsoft.AspNetCore.Identity;
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
        private readonly IdentityUserManager _identityUserManager;
        private readonly UserManager<IdentityUser> _userManager;

        public DispatcherManager(ApplicationDbContext dbContext,
                                 IdentityUserManager identityUserManager,
                                 UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
            _identityUserManager = identityUserManager;
            _userManager = userManager;
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

        public Dispatcher GetDispatcher(int id)
        {
            var dispatcherDto = _dbContext.Dispatchers.Where(d => d.Id == id).Include(d => d.User).FirstOrDefault();

            return _mapper.Map<DispatcherDTO, Dispatcher>(dispatcherDto);
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

        public async Task<IdentityResult> UpdateDispatcher(UpdateDispatcherModel model)
        {
            var dispatcher = GetDispatcher(model.Id);

            var passwordValidationResult = _identityUserManager.ValidatePassword(_userManager, dispatcher.User, model.Password);

            if (passwordValidationResult.Succeeded)
            {
                dispatcher.User.Email = model.NewEmail;
                dispatcher.User.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(dispatcher.User, model.Password);

                var updateResult = await _userManager.UpdateAsync(dispatcher.User);
                if (updateResult.Succeeded)
                {
                    await _dbContext.SaveChangesAsync();
                }
                return updateResult;
            }
            return passwordValidationResult;
        }

        public async Task DeleteDispatcher(int id)
        {
            var dispatcherDto = _dbContext.Dispatchers.Where(d => d.Id == id).Include(d => d.User).FirstOrDefault();
            
            _dbContext.Dispatchers.Remove(dispatcherDto);
            _dbContext.Users.Remove(dispatcherDto.User);
            await _dbContext.SaveChangesAsync();
        }
    }
}