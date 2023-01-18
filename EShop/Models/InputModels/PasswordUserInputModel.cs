using System.ComponentModel.DataAnnotations;

namespace Models.InputModels
{
    public  class PasswordUserInputModel
    {
        [Required]
        [StringLength(24, MinimumLength = 4)]
        public string Password { get; set; } = null!;

        [Required]
        [StringLength(24, MinimumLength = 4)]
        public string ConfimPassword { get; set; } = null!;
    }
}
