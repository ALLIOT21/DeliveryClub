using DeliveryClub.Domain.Models;
using DeliveryClub.Infrastructure.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace DeliveryClub.Infrastructure.Initialization
{
    public static class AppStart
    {
        public static async Task Run(IServiceProvider serviceProvider)
        {
            await InitializeUsersAndRoles(serviceProvider);
        }

        private static async Task InitializeUsersAndRoles(IServiceProvider serviceProvider)
        {            
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var superUser = serviceProvider.GetRequiredService<IOptions<SuperUser>>();

            await EnsureUsersAndRoles(userManager, roleManager, superUser.Value);           
        }

        private static async Task EnsureUsersAndRoles(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, SuperUser superUser)
        {
            await EnsureRoles(roleManager);
            await EnsureSuperUser(userManager, superUser);            
        }

        private static async Task EnsureRoles(RoleManager<IdentityRole> roleManager)
        {
            var roles = new[] { Role.Admin, Role.Dispatcher, Role.SuperUser, Role.User };

            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }

        private static async Task EnsureSuperUser(UserManager<IdentityUser> userManager, SuperUser superUser)
        {
            IdentityUser user = await userManager.FindByEmailAsync(superUser.Email);
            if (user != null)
            {
                await userManager.DeleteAsync(user);
            }

            user = new IdentityUser
            {
                UserName = superUser.Email,
                Email = superUser.Email,
            };

            IdentityResult result = await userManager.CreateAsync(user, superUser.Password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, Role.SuperUser);
            }
        }     
    }
}
