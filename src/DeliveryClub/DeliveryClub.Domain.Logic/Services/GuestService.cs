using DeliveryClub.Data.Context;
using DeliveryClub.Domain.Logic.Interfaces;
using DeliveryClub.Domain.Models;
using DeliveryClub.Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DeliveryClub.Domain.Logic.Services
{
    public class GuestService : IGuestService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public GuestService(ApplicationDbContext context, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _dbContext = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public string GetUserRole()
        {
            var userClaims = GetUserClaims();
            if (userClaims.IsUser())
            {
                return Role.User;
            }
            else
            if (userClaims.IsSuperUser())
            {
                return Role.SuperUser;
            }
            else
            if (userClaims.IsAdmin())
            {
                return Role.Admin;
            }
            else if (userClaims.IsDispatcher())
            {
                return Role.Dispatcher;
            }
            else
            {
                return null;
            }
        }

        private ClaimsPrincipal GetUserClaims()
        {
            return _signInManager.Context.User;
        }
    }
}
