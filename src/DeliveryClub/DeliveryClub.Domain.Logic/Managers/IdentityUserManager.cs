using DeliveryClub.Domain.AuxiliaryModels.SuperUser;
using DeliveryClub.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryClub.Domain.Logic.Managers
{
    public class IdentityUserManager
    {
        private readonly UserManager<IdentityUser> _userManager; 

        public IdentityUserManager(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<(IdentityUser, IdentityResult)> CreateIdentityUser(string email, string password, string role)
        {
            var iuser = new IdentityUser { UserName = email, Email = email };
            var passwordValidationResult = ValidatePassword(_userManager, iuser, password);
            if (passwordValidationResult.Succeeded)
            {
                var createResult = await _userManager.CreateAsync(iuser, password);
                if (createResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(iuser, role);
                }
                return (iuser, createResult);
            }
            return (iuser, passwordValidationResult);
        }

        public IdentityResult ValidatePassword(UserManager<IdentityUser> userManager, IdentityUser user, string password)
        {
            var passwordValidator = new PasswordValidator<IdentityUser>();
            var passwordValidationResult = passwordValidator.ValidateAsync(userManager, user, password);
            return passwordValidationResult.Result;
        }
    }
}
