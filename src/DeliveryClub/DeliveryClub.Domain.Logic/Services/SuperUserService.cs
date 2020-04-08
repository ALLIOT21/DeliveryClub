using DeliveryClub.Data.Context;

namespace DeliveryClub.Domain.Logic.Services
{
    public class SuperUserService
    {
        private readonly ApplicationDbContext _dbContext;

        public SuperUserService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


    }
}
