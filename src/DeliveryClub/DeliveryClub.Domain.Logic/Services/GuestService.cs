using DeliveryClub.Data.Context;
using DeliveryClub.Domain.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryClub.Domain.Logic.Services
{
    public class GuestService : IGuestService
    {
        private readonly ApplicationDbContext _dbContext;

        public GuestService(ApplicationDbContext context)
        {
            _dbContext = context;
        }

    }
}
