using System;
using System.Collections.Generic;

namespace Data
{
    /// <summary>
    /// Stores (names and addresses) that sell Adventure Works Cycles products to consumers.
    /// </summary>
    public partial class VStoreWithDemographic
    {
        public int CustomerId { get; set; }
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
        public string AddressType { get; set; } = null!;
        public string AddressLine1 { get; set; } = null!;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = null!;
        public string StateProvinceName { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string CountryRegionName { get; set; } = null!;
        public decimal? AnnualSales { get; set; }
        public decimal? AnnualRevenue { get; set; }
        public string? BankName { get; set; }
        public string? BusinessType { get; set; }
        public int? YearOpened { get; set; }
        public string? Specialty { get; set; }
        public int? SquareFeet { get; set; }
        public string? Brands { get; set; }
        public string? Internet { get; set; }
        public int? NumberEmployees { get; set; }
    }
}
