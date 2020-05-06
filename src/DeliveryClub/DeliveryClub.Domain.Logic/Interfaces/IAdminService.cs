using DeliveryClub.Domain.AuxiliaryModels.Admin;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DeliveryClub.Domain.Logic.Interfaces
{
    public interface IAdminService
    {
        Task<RestaurantInfoModel> GetRestaurantInfo(ClaimsPrincipal currentUser);

        Task<RestaurantInfoModel> UpdateRestaurantInfo(ClaimsPrincipal currentUser, RestaurantInfoModel restaurantInfoModel);

        Task<ProductGroupModel> CreateProductGroup(ClaimsPrincipal currentUser, ProductGroupModel model);

        Task<ICollection<ProductGroupModel>> GetProductGroups(ClaimsPrincipal currentUser);

        Task DeleteProductGroup(ClaimsPrincipal currentUser, int id);
    }
}
