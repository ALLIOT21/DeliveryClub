using DeliveryClub.Domain.AuxiliaryModels;
using DeliveryClub.Domain.AuxiliaryModels.SuperUser;
using DeliveryClub.Domain.Models.Actors;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeliveryClub.Domain.Logic.Interfaces
{
    public interface ISuperUserService
    {
        IEnumerable<GetAdminModel> GetAdmins();

        Task<UpdateAdminModel> GetAdmin(int id);

        Task<IdentityResult> CreateAdminAndRestaurant(CreateAdminModel model);

        Task<IdentityResult> UpdateAdmin(UpdateAdminModel model);

        Task DeleteAdminAndRestaurant(int id);
    }
}
