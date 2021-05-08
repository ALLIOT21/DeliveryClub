using Microsoft.AspNetCore.Identity;

namespace DeliveryClub.Data.DTO.ActorsDTO
{
    public class RegisteredUserDAO
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public IdentityUser User { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
    }
}
