using System;
using System.Collections.Generic;

namespace Data
{
    /// <summary>
    /// Cross-reference table mapping vendors and their employees.
    /// </summary>
    public partial class VendorContact
    {
        /// <summary>
        /// Primary key.
        /// </summary>
        public int VendorId { get; set; }
        /// <summary>
        /// Contact (Vendor employee) identification number. Foreign key to Contact.ContactID.
        /// </summary>
        public int ContactId { get; set; }
        /// <summary>
        /// Contact type such as sales manager, or sales agent.
        /// </summary>
        public int ContactTypeId { get; set; }
        /// <summary>
        /// Date and time the record was last updated.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        public virtual Contact Contact { get; set; } = null!;
        public virtual ContactType ContactType { get; set; } = null!;
        public virtual Vendor Vendor { get; set; } = null!;
    }
}
