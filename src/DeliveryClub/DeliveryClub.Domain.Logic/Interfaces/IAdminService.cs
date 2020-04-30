using DeliveryClub.Domain.AuxiliaryModels.Admin;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DeliveryClub.Domain.Logic.Interfaces
{
    public interface IAdminService
    {
        Task<RestaurantInfoModel> GetRestaurantInfo(ClaimsPrincipal currentUser);

        Task<RestaurantInfoModel> UpdateRestaurantInfo(ClaimsPrincipal currentUser, RestaurantInfoModel restaurantInfoModel); 
    }
}
