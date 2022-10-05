using System;
using System.Collections.Generic;

namespace Data
{
    /// <summary>
    /// Street address information for customers, employees, and vendors.
    /// </summary>
    public partial class Address
    {
        public Address()
        {
            CustomerAddresses = new HashSet<CustomerAddress>();
            EmployeeAddresses = new HashSet<EmployeeAddress>();
            SalesOrderHeaderBillToAddresses = new HashSet<SalesOrderHeader>();
            SalesOrderHeaderShipToAddresses = new HashSet<SalesOrderHeader>();
            VendorAddresses = new HashSet<VendorAddress>();
        }

        /// <summary>
        /// Primary key for Address records.
        /// </summary>
        public int AddressId { get; set; }
        /// <summary>
        /// First street address line.
        /// </summary>
        public string AddressLine1 { get; set; } = null!;
        /// <summary>
        /// Second street address line.
        /// </summary>
        public string? AddressLine2 { get; set; }
        /// <summary>
        /// Name of the city.
        /// </summary>
        public string City { get; set; } = null!;
        /// <summary>
        /// Unique identification number for the state or province. Foreign key to StateProvince table.
        /// </summary>
        public int StateProvinceId { get; set; }
        /// <summary>
        /// Postal code for the street address.
        /// </summary>
        public string PostalCode { get; set; } = null!;
        /// <summary>
        /// ROWGUIDCOL number uniquely identifying the record. Used to support a merge replication sample.
        /// </summary>
        public Guid Rowguid { get; set; }
        /// <summary>
        /// Date and time the record was last updated.
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        public virtual StateProvince StateProvince { get; set; } = null!;
        public virtual ICollection<CustomerAddress> CustomerAddresses { get; set; }
        public virtual ICollection<EmployeeAddress> EmployeeAddresses { get; set; }
        public virtual ICollection<SalesOrderHeader> SalesOrderHeaderBillToAddresses { get; set; }
        public virtual ICollection<SalesOrderHeader> SalesOrderHeaderShipToAddresses { get; set; }
        public virtual ICollection<VendorAddress> VendorAddresses { get; set; }
    }
}
