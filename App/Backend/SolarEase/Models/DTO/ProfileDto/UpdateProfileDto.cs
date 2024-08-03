using System.ComponentModel.DataAnnotations;

namespace SolarEase.Models.DTO.ProfileDto
{
    public class UpdateProfileDto
    {
        public IFormFile? Image { get; set; }
    }
}
