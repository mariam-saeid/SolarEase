using System.ComponentModel.DataAnnotations;

namespace SolarEase.Models.DTO.AccountDto
{
    public class ValidateCodeDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string ValidationCode { get; set; }

    }
}
