using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryClub.Domain.Logic.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<IdentityUser> GetCurrentIdentityUser(this UserManager<IdentityUser> userManager, ClaimsPrincipal currentUser)
        {
            var getUserResult = await userManager.GetUserAsync(currentUser);
            return getUserResult;
        }
    }
}
