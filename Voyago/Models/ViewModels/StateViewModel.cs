using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class StateViewModel
    {
        public StateViewModel(string code, string name)
        {
            StateProvinceCode = code;
            Name = name;
        }
        public string StateProvinceCode { get; set; }
        public string Name { get; set; }
    }
}
