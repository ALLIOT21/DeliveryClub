using DeliveryClub.Data.Context;
using DeliveryClub.Domain.Logic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DeliveryClub.Domain.Logic.Services
{
    public class GuestService : IGuestService
    {
        private readonly IApplicationDbContext _dbContext;

        public GuestService(IApplicationDbContext context)
        {
            _dbContext = context;
        }

    }
}
