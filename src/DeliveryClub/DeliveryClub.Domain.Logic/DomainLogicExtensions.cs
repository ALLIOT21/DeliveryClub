using Microsoft.Extensions.DependencyInjection;
using DeliveryClub.Data;
using Microsoft.Extensions.Configuration;
using DeliveryClub.Domain.Logic.Interfaces;
using DeliveryClub.Domain.Logic.Services;

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
            return services;
        }
    }
}