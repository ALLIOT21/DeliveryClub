using Microsoft.AspNetCore.Identity;

namespace DeliveryClub.Domain.Models.Actors
{
    public class RegisteredUser
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public IdentityUser User { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }
    }
}
