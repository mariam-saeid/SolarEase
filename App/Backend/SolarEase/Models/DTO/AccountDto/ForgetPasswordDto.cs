using System.ComponentModel.DataAnnotations;

namespace SolarEase.Models.DTO.AccountDto
{
    public class ForgetPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
