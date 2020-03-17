using Microsoft.EntityFrameworkCore;
using DeliveryClub.Data.DTO;

namespace DeliveryClub.Data.Contexts
{
    public class ProductDbContext : DbContext
    {
        public ProductDbContext(DbContextOptions<ProductDbContext> options)
            : base(options)
        {
        }

        public DbSet<ProductDTO> ProductDTO { get; set; }
    }
}
