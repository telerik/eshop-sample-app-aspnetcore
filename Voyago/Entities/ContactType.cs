using System;
using System.Collections.Generic;

namespace Data
{
    /// <summary>
    /// Lookup table containing the types of contacts stored in Contact.
    /// </summary>
    public partial class ContactType
    {
        public ContactType()
        {
            StoreContacts = new HashSet<StoreContact>();
            VendorContacts = new HashSet<VendorContact>();
        }

        /// <summary>
        /// Primary key for ContactType records.
        /// </summary>
        public int ContactTypeId { get; set; }
        /// <summary>
        /// Contact type description.
        /// </summary>
        public string Name { get; set; } = null!;
        /// <summary>
        /// Date and time the record was last updated.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<StoreContact> StoreContacts { get; set; }
        public virtual ICollection<VendorContact> VendorContacts { get; set; }
    }
}
