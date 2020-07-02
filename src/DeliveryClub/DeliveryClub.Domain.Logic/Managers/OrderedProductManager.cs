using DeliveryClub.Domain.AuxiliaryModels.Guest;
using DeliveryClub.Domain.Models.Entities;

namespace DeliveryClub.Domain.Logic.Managers
{
    public class OrderedProductManager
    {
        public OrderedProduct CreateOrderedProduct(OrderedProductModel model)
        {
            var op = new OrderedProduct
            {
                Amount = model.Amount,
                ProductId = model.Id,
                PortionPriceId = model.PortionId,
            };
            return op;
        }
    }
}
