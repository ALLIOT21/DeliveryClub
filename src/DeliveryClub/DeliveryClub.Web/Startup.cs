using AutoMapper;
using DeliveryClub.Data.Context;
using DeliveryClub.Domain.Logic;
using DeliveryClub.Infrastructure.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System.Globalization;

namespace DeliveryClub.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddDomainServices(Configuration);

            services.AddDefaultIdentity<IdentityUser>(options => {
                options.Password = new PasswordOptions
                {
                    RequireDigit = false,
                    RequiredLength = 2,
                    RequiredUniqueChars = 0,
                    RequireLowercase = false,
                    RequireUppercase = false,
                    RequireNonAlphanumeric = false,
                };
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders(); 

            services.AddRazorPages();
            services.AddOpenApiDocument();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddSignalR().AddAzureSignalR("Endpoint=https://deliveryclubsignal.service.signalr.net;AccessKey=S0Unq8T/ZsEBZTjYBlZT6CDjNs/+QH4Cfb+czxFRyRA=;Version=1.0;");

            services.Configure<SuperUser>(Configuration.GetSection("SuperUser"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }

            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();
            app.UseStaticFiles();

            app.UseRequestLocalization();
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Guest}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<DispatcherNotificationHub>("/DispatcherNotification");
            });
            
        }
    }
}