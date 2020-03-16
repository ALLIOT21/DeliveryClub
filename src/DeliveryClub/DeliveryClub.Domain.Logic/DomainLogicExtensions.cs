using Microsoft.Extensions.DependencyInjection;
using DeliveryClub.Data;

namespace DeliveryClub.Domain.Logic
{
    public static class DomainLogicExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddDataServices();
            //configure your Domain Logic Layer services here
            return services;
        }
    }
}