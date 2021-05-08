using DeliveryClub.Data.DTO.ActorsDTO;
using DeliveryClub.Data.DTO.EntitiesDTO;
using DeliveryClub.Domain.Models.Enumerations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DeliveryClub.Data.Context
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.Migrate();
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var orderStatusConverter = new EnumToNumberConverter<OrderStatus, int>();
            var specializationConverter = new EnumToNumberConverter<Specialization, int>();
            var paymentMethodConverter = new EnumToNumberConverter<PaymentMethod, int>();

            builder
                .Entity<SpecializationDAO>()
                .Property(s => s.Specialization)
                .HasConversion(specializationConverter);;

            builder
                .Entity<OrderedProductDAO>()
                .HasKey(o => new { o.ProductId, o.RestaurantOrderId, o.PortionPriceId });

            builder
                .Entity<PortionPriceProductsDAO>()
                .HasKey(o => new { o.PortionPriceId, o.ProductId });

            builder
                .Entity<PortionPriceProductGroupsDAO>()
                .HasKey(o => new { o.PortionPriceId, o.ProductGroupId });
        }

        public DbSet<OrderDAO> Orders { get; set; }

        public DbSet<OrderedProductDAO> OrderedProducts { get; set; }

        public DbSet<PortionPriceDAO> PortionPrices { get; set; }

        public DbSet<PortionPriceProductsDAO> PortionPriceProducts { get; set; }

        public DbSet<PortionPriceProductGroupsDAO> PortionPriceProductGroups { get; set; }

        public DbSet<ProductDAO> Products { get; set; }

        public DbSet<ProductGroupDAO> ProductGroups { get; set; }

        public DbSet<RestaurantAdditionalInfoDAO> RestaurantAdditionalInfos { get; set; }

        public DbSet<RestaurantDAO> Restaurants { get; set; }

        public DbSet<SpecializationDAO> Specializations { get; set; }

        public DbSet<AdminDAO> Admins { get; set; }

        public DbSet<DispatcherDAO> Dispatchers { get; set; }

        public DbSet<RegisteredUserDAO> RegisteredUsers { get; set; }

        public DbSet<RestaurantOrderDAO> RestaurantOrders { get; set; } 
    }
}