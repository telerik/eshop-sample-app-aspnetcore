using System;
using System.Collections.Generic;

namespace Data
{
    /// <summary>
    /// Cross-reference mapping vendors and addresses.
    /// </summary>
    public partial class VendorAddress
    {
        /// <summary>
        /// Primary key. Foreign key to Vendor.VendorID.
        /// </summary>
        public int VendorId { get; set; }
        /// <summary>
        /// Primary key. Foreign key to Address.AddressID.
        /// </summary>
        public int AddressId { get; set; }
        /// <summary>
        /// Address type. Foreign key to AddressType.AddressTypeID.
        /// </summary>
        public int AddressTypeId { get; set; }
        /// <summary>
        /// Date and time the record was last updated.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        public virtual Address Address { get; set; } = null!;
        public virtual AddressType AddressType { get; set; } = null!;
        public virtual Vendor Vendor { get; set; } = null!;
    }
}
