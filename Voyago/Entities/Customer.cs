using System;
using System.Collections.Generic;

namespace Data
{
    /// <summary>
    /// Current customer information. Also see the Individual and Store tables.
    /// </summary>
    public partial class Customer
    {
        public Customer()
        {
            CustomerAddresses = new HashSet<CustomerAddress>();
            SalesOrderHeaders = new HashSet<SalesOrderHeader>();
        }

        /// <summary>
        /// Primary key for Customer records.
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// ID of the territory in which the customer is located. Foreign key to SalesTerritory.SalesTerritoryID.
        /// </summary>
        public int? TerritoryId { get; set; }
        /// <summary>
        /// Unique number identifying the customer assigned by the accounting system.
        /// </summary>
        public string AccountNumber { get; set; } = null!;
        /// <summary>
        /// Customer type: I = Individual, S = Store
        /// </summary>
        public string CustomerType { get; set; } = null!;
        /// <summary>
        /// ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.
        /// </summary>
        public Guid Rowguid { get; set; }
        /// <summary>
        /// Date and time the record was last updated.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        public virtual SalesTerritory? Territory { get; set; }
        public virtual Individual Individual { get; set; } = null!;
        public virtual Store Store { get; set; } = null!;
        public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; }
        public virtual ICollection<SalesOrderHeader> SalesOrderHeaders { get; set; }
    }
}
