using DeliveryClub.Domain.Models.Enumerations;

namespace DeliveryClub.Data.DTO.EntitiesDTO
{
    public class PaymentMethodDTO
    {
        public int Id { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public int RestaurantAdditionalInfoId { get; set; }

        public RestaurantAdditionalInfoDTO RestaurantAdditionalInfo { get; set; }
    }
}
