using System;
using System.Collections.Generic;

namespace Data
{
    /// <summary>
    /// Sales representative transfers to other sales territories.
    /// </summary>
    public partial class SalesTerritoryHistory
    {
        /// <summary>
        /// Primary key for SalesTerritoryHistory records.
        /// </summary>
        public int SalesPersonId { get; set; }
        /// <summary>
        /// Territory identification number. Foreign key to SalesTerritory.SalesTerritoryID.
        /// </summary>
        public int TerritoryId { get; set; }
        /// <summary>
        /// Date the sales representive started work in the territory.
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Date the sales representative left work in the territory.
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.
        /// </summary>
        public Guid Rowguid { get; set; }
        /// <summary>
        /// Date and time the record was last updated.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        public virtual SalesPerson SalesPerson { get; set; } = null!;
        public virtual SalesTerritory Territory { get; set; } = null!;
    }
}
