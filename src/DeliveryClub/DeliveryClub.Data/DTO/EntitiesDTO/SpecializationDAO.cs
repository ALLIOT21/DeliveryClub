using DeliveryClub.Domain.Models.Enumerations;

namespace DeliveryClub.Data.DTO.EntitiesDTO
{
    public class SpecializationDAO
    {
        public int Id { get; set; }

        public Specialization Specialization { get; set; }

        public int RestaurantId { get; set; }

        public RestaurantDAO Restaurant { get; set; }
    }
}
