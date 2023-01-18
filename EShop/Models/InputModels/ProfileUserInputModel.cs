using System.ComponentModel.DataAnnotations;

namespace Models.InputModels
{
    public class ProfileUserInputModel
    {
        [Required]
        public string FirstName { get; set; } = null!;

        [Required]
        public string LastName { get; set; } = null!;

        public string? Phone { get; set; }
    }
}
