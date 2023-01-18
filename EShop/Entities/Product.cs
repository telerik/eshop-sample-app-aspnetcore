using System;
using System.Collections.Generic;

namespace Data
{
    public partial class Product
    {
        public Product()
        {
            SalesOrders = new HashSet<SalesOrder>();
            ShoppingCartItems = new HashSet<ShoppingCartItem>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string ProductNumber { get; set; } = null!;
        public int? ProductModelId { get; set; }
        public string? ProductModel { get; set; }
        public decimal ListPrice { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public decimal? Weight { get; set; }
        public decimal? DiscountPct { get; set; }
        public int? ProductCategoryId { get; set; }
        public string? ProductCategoryName { get; set; }
        public int? ProductSubcategoryId { get; set; }
        public string? ProductSubCategoryName { get; set; }
        public string? Description { get; set; }
        public short? Quantity { get; set; }
        public int? Rating { get; set; }
        public int? PhotoId { get; set; }
        public byte[]? ThumbNailPhoto { get; set; }
        public byte[]? LargePhoto { get; set; }

        public virtual ICollection<SalesOrder> SalesOrders { get; set; }
        public virtual ICollection<ShoppingCartItem> ShoppingCartItems { get; set; }
    }
}
