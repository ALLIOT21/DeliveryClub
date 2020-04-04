using DeliveryClub.Data.DTO.EntitiesDTO;
using DeliveryClub.Data.DTO.ActorsDTO;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DeliveryClub.Data.DTO.Enumerations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DeliveryClub.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext, IApplicationDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var specializationConverter = new EnumToNumberConverter<Specialization, int>();
            var paymentMethodConverter = new EnumToNumberConverter<PaymentMethod, int>();

            builder
                .Entity<SpecializationDTO>()
                .Property(s => s.Specialization)
                .HasConversion(specializationConverter);

            builder
                .Entity<PaymentMethodDTO>()
                .Property(p => p.PaymentMethod)
                .HasConversion(paymentMethodConverter);

            builder
                .Entity<OrderedProductDTO>()
                .HasKey(o => new { o.ProductId, o.OrderId, o.PortionPriceId });
        }

        public DbSet<OrderDTO> Orders { get; set; }

        public DbSet<OrderedProductDTO> OrderedProducts { get; set; }

        public DbSet<PortionPriceDTO> PortionPrices { get; set; }

        public DbSet<ProductDTO> Products { get; set; }

        public DbSet<ProductGroupDTO> ProductGroups { get; set; }

        public DbSet<RestaurantAdditionalInfoDTO> RestaurantAdditionalInfos { get; set; }

        public DbSet<RestaurantDTO> Restaurants { get; set; }

        public DbSet<ReviewDTO> Reviews { get; set; }
    }
}
