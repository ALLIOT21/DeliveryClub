using DeliveryClub.Domain.AuxiliaryModels.Admin;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DeliveryClub.Domain.Logic.Interfaces
{
    public interface IAdminService
    {
        Task<RestaurantInfoModel> GetRestaurantInfo();

        Task<RestaurantInfoModel> UpdateRestaurantInfo(RestaurantInfoModel restaurantInfoModel);

        Task<ProductGroupModel> CreateProductGroup(ProductGroupModel model);

        Task<ICollection<ProductGroupModel>> GetProductGroups();

        Task DeleteProductGroup(int id);

        Task UpdateProductGroup(ProductGroupModel model);

        Task CreateProduct(ProductModel model);

        ProductModel GetProduct(int id);

        Task<ProductModel> UpdateProduct(ProductModel productModel);

        Task DeleteProduct(int id);

        Task<bool> HasPortionPrices(string productGroupName);
    }
}
