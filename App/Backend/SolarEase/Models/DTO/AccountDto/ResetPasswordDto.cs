using System.ComponentModel.DataAnnotations;

namespace SolarEase.Models.DTO.AccountDto
{
    public class ResetPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
