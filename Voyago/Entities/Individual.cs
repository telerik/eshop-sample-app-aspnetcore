using System;
using System.Collections.Generic;

namespace Data
{
    /// <summary>
    /// Demographic data about customers that purchase Adventure Works products online.
    /// </summary>
    public partial class Individual
    {
        /// <summary>
        /// Unique customer identification number. Foreign key to Customer.CustomerID.
        /// </summary>
        public int CustomerId { get; set; }
        /// <summary>
        /// Identifies the customer in the Contact table. Foreign key to Contact.ContactID.
        /// </summary>
        public int ContactId { get; set; }
        /// <summary>
        /// Personal information such as hobbies, and income collected from online shoppers. Used for sales analysis.
        /// </summary>
        public string? Demographics { get; set; }
        /// <summary>
        /// Date and time the record was last updated.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        public virtual Contact Contact { get; set; } = null!;
        public virtual Customer Customer { get; set; } = null!;
    }
}
