using System;
using System.Collections.Generic;

namespace Data
{
    public partial class SalesOrder
    {
        public int OrderId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Total { get; set; }
        public byte Status { get; set; }
        public DateTime? ShipDate { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public int ContactId { get; set; }
        public decimal LineTotal { get; set; }
        public int? ProductId { get; set; }
        public int? OrderNumber { get; set; }

        public virtual Contact Contact { get; set; } = null!;
        public virtual Product? Product { get; set; }
    }
}
