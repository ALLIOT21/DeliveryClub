using DeliveryClub.Domain.Models.Enumerations;

namespace DeliveryClub.Data.DTO.EntitiesDTO
{
    public class SpecializationDTO
    {
        public int Id { get; set; }

        public Specialization Specialization { get; set; }

        public int RestaurantId { get; set; }

        public RestaurantDTO Restaurant { get; set; }
    }
}
