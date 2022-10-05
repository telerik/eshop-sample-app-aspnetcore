using System;
using System.Collections.Generic;

namespace Data
{
    /// <summary>
    /// Cross-reference table mapping stores and their employees.
    /// </summary>
    public partial class StoreContact
    {
        /// <summary>
        /// Store identification number. Foreign key to Customer.CustomerID.
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// Contact (store employee) identification number. Foreign key to Contact.ContactID.
        /// </summary>
        public int ContactId { get; set; }
        /// <summary>
        /// Contact type such as owner or purchasing agent. Foreign key to ContactType.ContactTypeID.
        /// </summary>
        public int ContactTypeId { get; set; }
        /// <summary>
        /// ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.
        /// </summary>
        public Guid Rowguid { get; set; }
        /// <summary>
        /// Date and time the record was last updated.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        public virtual Contact Contact { get; set; } = null!;
        public virtual ContactType ContactType { get; set; } = null!;
        public virtual Store Customer { get; set; } = null!;
    }
}
