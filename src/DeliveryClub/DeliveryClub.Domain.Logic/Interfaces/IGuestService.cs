using DeliveryClub.Domain.AuxiliaryModels.Guest;
using System.Collections.Generic;

namespace DeliveryClub.Domain.Logic.Interfaces
{
    public interface IGuestService
    {
        string GetUserRole();

        ICollection<RestaurantPartialModel> GetRestaurantsPartially();

        RestaurantFullModel GetRestaurantFull(int id);
    }
}
