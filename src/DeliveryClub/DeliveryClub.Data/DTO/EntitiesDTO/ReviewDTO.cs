namespace DeliveryClub.Data.DTO.EntitiesDTO
{
    public class ReviewDTO
    {
        public int Id { get; set; }

        public int ReviewAmount { get; set; }

        public double ReviewOverallRating { get; set; }

        public int RestaurantId { get; set; }

        public RestaurantDTO Restaurant { get; set; }
    }
}
