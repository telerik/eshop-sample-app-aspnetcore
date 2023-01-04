using System.ComponentModel.DataAnnotations;

namespace Models.InputModels
{
    public class LoginUserInpuModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(24, MinimumLength = 4)]
        public string Password { get; set; } = null!;
    }
}
