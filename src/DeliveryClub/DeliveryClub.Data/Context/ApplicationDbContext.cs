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
                .Entity<SpecializationDTO>()
                .Property(s => s.Specialization)
                .HasConversion(specializationConverter);;

            builder
                .Entity<OrderedProductDTO>()
                .HasKey(o => new { o.ProductId, o.RestaurantOrderId, o.PortionPriceId });

            builder
                .Entity<PortionPriceProductsDTO>()
                .HasKey(o => new { o.PortionPriceId, o.ProductId });

            builder
                .Entity<PortionPriceProductGroupsDTO>()
                .HasKey(o => new { o.PortionPriceId, o.ProductGroupId });
        }

        public DbSet<OrderDTO> Orders { get; set; }

        public DbSet<OrderedProductDTO> OrderedProducts { get; set; }

        public DbSet<PortionPriceDTO> PortionPrices { get; set; }

        public DbSet<PortionPriceProductsDTO> PortionPriceProducts { get; set; }

        public DbSet<PortionPriceProductGroupsDTO> PortionPriceProductGroups { get; set; }

        public DbSet<ProductDTO> Products { get; set; }

        public DbSet<ProductGroupDTO> ProductGroups { get; set; }

        public DbSet<RestaurantAdditionalInfoDTO> RestaurantAdditionalInfos { get; set; }

        public DbSet<RestaurantDTO> Restaurants { get; set; }

        public DbSet<SpecializationDTO> Specializations { get; set; }

        public DbSet<AdminDTO> Admins { get; set; }

        public DbSet<DispatcherDTO> Dispatchers { get; set; }

        public DbSet<RegisteredUserDTO> RegisteredUsers { get; set; }

        public DbSet<RestaurantOrderDTO> RestaurantOrders { get; set; } 
    }
}