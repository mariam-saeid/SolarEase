using System.ComponentModel.DataAnnotations;

namespace SolarEase.Models.DTO.PersonDto
{
    public class UpdatePersonInfoDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public string? Password { get; set; }
        [Required]
        [MaxLength(20, ErrorMessage = "Name cannot be over 20 over characters")]
        public string Name { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "Location cannot be over 30 over characters")]
        public string Location { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Phone number must be between 10 and 15 characters.")]
        public string PhoneNumber { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "SystemSize must be between 0 to 100.")]
        public double SystemSize { get; set; }
        public IFormFile? Image { get; set; }
        [Required]
        public bool ImageUpdated { get; set; }
    }
}
