using DeliveryClub.Domain.Models.Enumerations;

namespace DeliveryClub.Web.ViewModels.Dispatchers
{
    public class SetOrderStatusViewModel
    {
        public int Id { get; set; }

        public OrderStatus OrderStatus { get; set; }
    }
}
