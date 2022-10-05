using System;
using System.Collections.Generic;

namespace Data
{
    /// <summary>
    /// Sales performance tracking.
    /// </summary>
    public partial class SalesPersonQuotaHistory
    {
        /// <summary>
        /// Sales person identification number. Foreign key to SalesPerson.SalesPersonID.
        /// </summary>
        public int SalesPersonId { get; set; }
        /// <summary>
        /// Sales quota date.
        /// </summary>
        public DateTime QuotaDate { get; set; }
        /// <summary>
        /// Sales quota amount.
        /// </summary>
        public decimal SalesQuota { get; set; }
        /// <summary>
        /// ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.
        /// </summary>
        public Guid Rowguid { get; set; }
        /// <summary>
        /// Date and time the record was last updated.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        public virtual SalesPerson SalesPerson { get; set; } = null!;
    }
}
