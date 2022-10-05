using System;
using System.Collections.Generic;

namespace Data
{
    /// <summary>
    /// Vendor (company) names and addresses and the names of vendor employees to contact.
    /// </summary>
    public partial class VVendor
    {
        public int VendorId { get; set; }
        public string Name { get; set; } = null!;
        public string ContactType { get; set; } = null!;
        public string? Title { get; set; }
        public string FirstName { get; set; } = null!;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = null!;
        public string? Suffix { get; set; }
        public string? Phone { get; set; }
        public string? EmailAddress { get; set; }
        public int EmailPromotion { get; set; }
        public string AddressLine1 { get; set; } = null!;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = null!;
        public string StateProvinceName { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string CountryRegionName { get; set; } = null!;
    }
}
