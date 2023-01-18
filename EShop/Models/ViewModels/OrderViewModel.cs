using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class OrderViewModel
    {
        public int OrderID { get; set; }
        public int? OrderNumber { get; set; }
        public decimal Total { get; set; }
        public int Status { get; set; }
        public DateTime? OrderDate { get; set; }
    }
}
