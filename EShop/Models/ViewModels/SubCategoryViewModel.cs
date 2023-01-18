namespace Models.ViewModels
{
    public class SubCategoryViewModel
    {
        public int? SubCategoryId { get; set; }

        public int? CategoryId { get; set; }

        public string Name { get; set; } = null!;

        public string? ImageName { get; set; } = null!;

        public int? TotalSales { get; set; }
    }
}
