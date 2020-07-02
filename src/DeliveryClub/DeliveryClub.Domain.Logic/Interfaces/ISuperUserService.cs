using DeliveryClub.Domain.AuxiliaryModels;
using DeliveryClub.Domain.AuxiliaryModels.Admin;
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

        Task<ICollection<GetDispatcherModel>> GetDispatchers();

        Task<IdentityResult> CreateDispatcher(CreateDispatcherModel model);

        UpdateDispatcherModel GetDispatcher(int id);

        Task<IdentityResult> UpdateDispatcher(UpdateDispatcherModel model);

        Task DeleteDispatcher(int id);

        Task ToggleDispatcherActivity(int id);
    }
}
