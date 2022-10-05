namespace Models.ViewModels
{
    public class FavoriteProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public decimal DiscountPct { get; set; }

        public decimal FinalPrice => Price * (1 - DiscountPct);

        public double AverageRating { get; set; }

        public string Description { get; set; } = null!;

        public int? PhotoId { get; set; }

        public int? ModelId { get; set; }
    }
}
