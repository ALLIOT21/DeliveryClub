using DeliveryClub.Domain.Models;
using System.Security.Claims;

namespace DeliveryClub.Infrastructure.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static bool IsSuperUser(this ClaimsPrincipal user)
        {
            return user.IsInRole(Role.SuperUser);
        }

        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            return user.IsInRole(Role.Admin);
        }

        public static bool IsDispatcher(this ClaimsPrincipal user)
        {
            return user.IsInRole(Role.Dispatcher);
        }

        public static bool IsUser(this ClaimsPrincipal user)
        {
            return user.IsInRole(Role.User);
        }
    }
}

