using DeliveryClub.Data.Context;
using DeliveryClub.Data.DTO.ActorsDTO;
using DeliveryClub.Domain.Models.Actors;
using DeliveryClub.Infrastructure.Mapping;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Reflection;

namespace DeliveryClub.Domain.Logic.Managers
{
    public class AdminManager
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Mapper _mapper;

        public AdminManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _mapper = new Mapper(Assembly.GetExecutingAssembly());
        }

        public Admin GetAdmin(string id)
        {
            var admin = _dbContext.Admins.Where(a => a.UserId == id).FirstOrDefault();
            return _mapper.Map<AdminDTO, Admin>(admin);
        }
    }
}
