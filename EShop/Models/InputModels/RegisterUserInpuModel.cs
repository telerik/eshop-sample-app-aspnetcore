using System.ComponentModel.DataAnnotations;

namespace Models.InputModels
{
    public class RegisterUserInpuModel
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z]+\s+[a-zA-Z]+$", 
            ErrorMessage = "First and last name should be separated by whitespace")]
        public string FirstAndLastName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(24,MinimumLength = 8)]
        public string Password { get; set; } = null!;
    }
}
