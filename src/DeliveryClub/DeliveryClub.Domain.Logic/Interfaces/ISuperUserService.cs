using DeliveryClub.Domain.AuxiliaryModels;
using DeliveryClub.Domain.Models.Actors;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeliveryClub.Domain.Logic.Interfaces
{
    public interface ISuperUserService
    {
        Task<IdentityResult> CreateAdminAndRestaurant(CreateAdminModel model);

        IEnumerable<Admin> GetAdmins();
    }
}
