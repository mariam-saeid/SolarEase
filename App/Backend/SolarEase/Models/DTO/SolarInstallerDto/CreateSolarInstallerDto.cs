using System.ComponentModel.DataAnnotations;

namespace SolarEase.Models.DTO.SolarInstallerDto
{
    public class CreateSolarInstallerDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
    }
}
