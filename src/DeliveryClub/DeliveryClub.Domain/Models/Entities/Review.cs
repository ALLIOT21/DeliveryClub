namespace DeliveryClub.Domain.Models.Entities
{
    public class Review
    {
        public int Id { get; set; }

        public int ReviewAmount { get; set; }

        public double ReviewOverallRating { get; set; }

        public int RestaurantId { get; set; }

        public Restaurant Restaurant { get; set; }
    }
}
