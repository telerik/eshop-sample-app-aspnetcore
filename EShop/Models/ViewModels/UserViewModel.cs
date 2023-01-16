using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public bool NameStyle { get; set; }

        public string? Title { get; set; }

        public string FirstName { get; set; } = null!;

        public string? MiddleName { get; set; }

        public string LastName { get; set; } = null!;

        public string? Suffix { get; set; }

        public string EmailAddress { get; set; } = null!;

        public int EmailPromotion { get; set; }

        public string? Phone { get; set; }

        public string? AdditionalContactInfo { get; set; }
    }
}
