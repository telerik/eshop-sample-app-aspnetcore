using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class UserProfileViewModel
    {
        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string? EmailAddress { get; set; }

        public string? Phone { get; set; }

        public string Password { get; set; } = null!;

        public string ConfimPassword { get; set; } = null!;

        public string? Street { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? Zipcode { get; set; }

        public string? Country { get; set; }
    }
}
