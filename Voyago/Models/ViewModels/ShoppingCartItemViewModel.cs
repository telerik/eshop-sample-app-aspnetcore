using System.ComponentModel.DataAnnotations;

namespace Models.ViewModels
{
    public class ShoppingCartItemViewModel
    {
        public int ShoppingCartItemId { get; set; }

        [Required]
        public string ShoppingCartId { get; set; } = null!;

        [Required]
        public int ProductId { get; set; }

        [Display(Name = "Product")]
        public string ProductName { get; set; } = null!;

        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        public decimal ProductPrice { get; set; }

        [DataType("Integer")]
        [Range(1, 100)]
        public int Quantity { get; set; }

        [DataType("float")]
        public decimal? DiscountPcnt { get; set; }

        [Display(Name = "Final Price")]
        [DataType(DataType.Currency)]
        public decimal Total => Math.Round(ProductPrice * (1 - (decimal)DiscountPcnt) * Quantity, 2);

        public int? ProductPhotoId { get; set; }
    }
}