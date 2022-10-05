using System;
using System.Collections.Generic;

namespace Data
{
    /// <summary>
    /// Uses PIVOT to return aggregated sales information for each sales representative.
    /// </summary>
    public partial class VSalesPersonSalesByFiscalYear
    {
        public int? SalesPersonId { get; set; }
        public string? FullName { get; set; }
        public string Title { get; set; } = null!;
        public string SalesTerritory { get; set; } = null!;
        public decimal? _2002 { get; set; }
        public decimal? _2003 { get; set; }
        public decimal? _2004 { get; set; }
    }
}
