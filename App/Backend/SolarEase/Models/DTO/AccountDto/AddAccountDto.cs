using System.ComponentModel.DataAnnotations;

namespace SolarEase.Models.DTO.AccountDto
{
    public class AddAccountDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Type { get; set; }
    }
}
