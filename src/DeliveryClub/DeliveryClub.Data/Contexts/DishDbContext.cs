using Microsoft.EntityFrameworkCore;
using DeliveryClub.Data.DTO;

namespace DeliveryClub.Data.Contexts
{
    public class DishDbContext : DbContext
    {
        public DishDbContext(DbContextOptions<DishDbContext> options)
            : base(options)
        {
        }

        public DbSet<DishDTO> DishDTO { get; set; }
    }
}
