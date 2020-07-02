using Microsoft.Extensions.DependencyInjection;
using DeliveryClub.Data;
using Microsoft.Extensions.Configuration;
using DeliveryClub.Domain.Logic.Interfaces;
using DeliveryClub.Domain.Logic.Services;
using DeliveryClub.Domain.Logic.Managers;
using DeliveryClub.Domain.Logic.Mapping;

namespace DeliveryClub.Domain.Logic
{
    public static class DomainLogicExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDataServices(configuration);
            services.AddScoped<IGuestService, GuestService>();
            services.AddScoped<ISuperUserService, SuperUserService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<AdminManager>();
            services.AddScoped<DispatcherManager>();
            services.AddScoped<IdentityUserManager>();
            services.AddScoped<PaymentMethodManager>();
            services.AddScoped<PortionPriceManager>();
            services.AddScoped<PortionPriceProductGroupManager>();
            services.AddScoped<PortionPriceProductManager>();
            services.AddScoped<ProductGroupManager>();
            services.AddScoped<ProductManager>();
            services.AddScoped<RestaurantAdditionalInfoManager>();
            services.AddScoped<RestaurantManager>();
            services.AddScoped<SpecializationManager>();
            services.AddScoped<OrderManager>();
            services.AddScoped<RestaurantOrderManager>();
            services.AddScoped<OrderedProductManager>();
            services.AddScoped<AuxiliaryMapper>();           
            return services;
        }
    }
}