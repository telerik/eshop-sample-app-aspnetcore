using System;
using System.Collections.Generic;

namespace Data
{
    /// <summary>
    /// Cross-reference table mapping employees to their address(es).
    /// </summary>
    public partial class EmployeeAddress
    {
        /// <summary>
        /// Primary key. Foreign key to Employee.EmployeeID.
        /// </summary>
        public int EmployeeId { get; set; }
        /// <summary>
        /// Primary key. Foreign key to Address.AddressID.
        /// </summary>
        public int AddressId { get; set; }
        /// <summary>
        /// ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.
        /// </summary>
        public Guid Rowguid { get; set; }
        /// <summary>
        /// Date and time the record was last updated.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        public virtual Address Address { get; set; } = null!;
        public virtual Employee Employee { get; set; } = null!;
    }
}
