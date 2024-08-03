using System.ComponentModel.DataAnnotations;

namespace SolarEase.Models.DTO.AccountDto
{
    public class UpdateAccountDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string? Password { get; set; }
    }
}
