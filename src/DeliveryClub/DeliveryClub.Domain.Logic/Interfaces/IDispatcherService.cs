using DeliveryClub.Domain.AuxiliaryModels.Dispatcher;
using DeliveryClub.Domain.Models.Enumerations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DeliveryClub.Domain.Logic.Interfaces
{
    public interface IDispatcherService
    {
        Task<ICollection<DispatcherOrderModel>> GetOrders(OrderStatus orderStatus);
    }
}
