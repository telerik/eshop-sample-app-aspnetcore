using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class ReportSourceModel
    {
        public string ReportId { get; set; }
        public Dictionary<string, object> Parameters { get; set; }
    }
}
