using System.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DeliveryClub.Data.Contexts;

namespace DeliveryClub.Data
{
    public static class DataExtensions
    {
        public static IServiceCollection AddDataServices(this IServiceCollection services)
        {
            services.AddDbContext<DishDbContext>(options => options.UseSqlServer(ConfigurationManager.ConnectionStrings["DeliveryClub"].ConnectionString));

            return services;
        }
    }
}