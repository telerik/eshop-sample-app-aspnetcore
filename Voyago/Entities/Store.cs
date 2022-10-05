using System;
using System.Collections.Generic;

namespace Data
{
    /// <summary>
    /// Customers (resellers) of Adventure Works products.
    /// </summary>
    public partial class Store
    {
        public Store()
        {
            StoreContacts = new HashSet<StoreContact>();
        }

        /// <summary>
        /// Primary key. Foreign key to Customer.CustomerID.
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// Name of the store.
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// ID of the sales person assigned to the customer. Foreign key to SalesPerson.SalesPersonID.
        /// </summary>
        public int? SalesPersonId { get; set; }
        /// <summary>
        /// Demographic informationg about the store such as the number of employees, annual sales and store type.
        /// </summary>
        public string? Demographics { get; set; }
        /// <summary>
        /// ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.
        /// </summary>
        public Guid Rowguid { get; set; }
        /// <summary>
        /// Date and time the record was last updated.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual SalesPerson? SalesPerson { get; set; }
        public virtual ICollection<StoreContact> StoreContacts { get; set; }
    }
}
