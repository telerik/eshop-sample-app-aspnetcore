using System;
using System.Collections.Generic;

namespace Data
{
    /// <summary>
    /// Names of each employee, customer contact, and vendor contact.
    /// </summary>
    public partial class Contact
    {
        public Contact()
        {
            ContactCreditCards = new HashSet<ContactCreditCard>();
            Employees = new HashSet<Employee>();
            Individuals = new HashSet<Individual>();
            SalesOrderHeaders = new HashSet<SalesOrderHeader>();
            StoreContacts = new HashSet<StoreContact>();
            VendorContacts = new HashSet<VendorContact>();
        }

        /// <summary>
        /// Primary key for Contact records.
        /// </summary>
        public int ContactId { get; set; }
        /// <summary>
        /// 0 = The data in FirstName and LastName are stored in western style (first name, last name) order.  1 = Eastern style (last name, first name) order.
        /// </summary>
        public bool NameStyle { get; set; }
        /// <summary>
        /// A courtesy title. For example, Mr. or Ms.
        /// </summary>
        public string? Title { get; set; }
        /// <summary>
        /// First name of the person.
        /// </summary>
        public string FirstName { get; set; } = null!;
        /// <summary>
        /// Middle name or middle initial of the person.
        /// </summary>
        public string? MiddleName { get; set; }
        /// <summary>
        /// Last name of the person.
        /// </summary>
        public string LastName { get; set; } = null!;
        /// <summary>
        /// Surname suffix. For example, Sr. or Jr.
        /// </summary>
        public string? Suffix { get; set; }
        /// <summary>
        /// E-mail address for the person.
        /// </summary>
        public string EmailAddress { get; set; } = null!;
        /// <summary>
        /// 0 = Contact does not wish to receive e-mail promotions, 1 = Contact does wish to receive e-mail promotions from AdventureWorks, 2 = Contact does wish to receive e-mail promotions from AdventureWorks and selected partners. 
        /// </summary>
        public int EmailPromotion { get; set; }
        /// <summary>
        /// Phone number associated with the person.
        /// </summary>
        public string? Phone { get; set; }
        /// <summary>
        /// Password for the e-mail account.
        /// </summary>
        public string PasswordHash { get; set; } = null!;
        /// <summary>
        /// Random value concatenated with the password string before the password is hashed.
        /// </summary>
        public string PasswordSalt { get; set; } = null!;
        /// <summary>
        /// Additional contact information about the person stored in xml format. 
        /// </summary>
        public string? AdditionalContactInfo { get; set; }
        /// <summary>
        /// ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.
        /// </summary>
        public Guid Rowguid { get; set; }
        /// <summary>
        /// Date and time the record was last updated.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        public virtual ICollection<ContactCreditCard> ContactCreditCards { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }
        public virtual ICollection<Individual> Individuals { get; set; }
        public virtual ICollection<SalesOrderHeader> SalesOrderHeaders { get; set; }
        public virtual ICollection<StoreContact> StoreContacts { get; set; }
        public virtual ICollection<VendorContact> VendorContacts { get; set; }
    }
}
