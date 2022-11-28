using System;
using System.Collections.Generic;

namespace Data
{
    public partial class Contact
    {
        public Contact()
        {
            SalesOrders = new HashSet<SalesOrder>();
        }

        public int ContactId { get; set; }
        public string EmailAddress { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Phone { get; set; }
        public string PasswordHash { get; set; } = null!;
        public string PasswordSalt { get; set; } = null!;
        public string? City { get; set; }
        public string? ZipCode { get; set; }
        public string? Street { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }

        public virtual ICollection<SalesOrder> SalesOrders { get; set; }
    }
}
