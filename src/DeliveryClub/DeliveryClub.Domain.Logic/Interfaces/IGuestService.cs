using DeliveryClub.Domain.AuxiliaryModels.Admin;
using DeliveryClub.Domain.AuxiliaryModels.Guest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeliveryClub.Domain.Logic.Interfaces
{
    public interface IGuestService
    {
        string GetUserRole();

        ICollection<RestaurantPartialModel> GetRestaurantsPartially();

        RestaurantFullModel GetRestaurantFull(int id);

        List<NamePaymentModel> GetRestaurantNamePayments(List<int> restaurantIds);

        Task<int> CreateOrder(CreateOrderModel model);
    }
}
