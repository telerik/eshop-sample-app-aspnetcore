namespace Models.ViewModels
{
    public  class ProductDetailsViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string ProductNumber { get; set; } = null!;

        public string? Description { get; set; }

        public int? ModelId { get; set; }

        public string? Model { get; set; }

        public string? Color { get; set; }

        public decimal ListPrice { get; set; }

        public decimal? DiscountPct { get; set; }

        public decimal? FinalPrice => ListPrice * (1 - DiscountPct);

        public string? Size { get; set; }

        public decimal? Weight { get; set; }

        public int? CategoryId { get; set; }

        public string? Category { get; set; }

        public int? SubCategoryId { get; set; }

        public string? SubCategory { get; set; }

        public double? AverageRating { get; set; }

        public short? InStock { get; set; }

        public int? PhotoId { get; set; }
    }
}