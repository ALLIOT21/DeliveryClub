using DeliveryClub.Data.DTO.EntitiesDTO;
using Microsoft.AspNetCore.Identity;

namespace DeliveryClub.Data.DTO.ActorsDTO
{
    public class DispatcherDAO
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public IdentityUser User { get; set; }

        public bool IsActive { get; set; }
    }
}
