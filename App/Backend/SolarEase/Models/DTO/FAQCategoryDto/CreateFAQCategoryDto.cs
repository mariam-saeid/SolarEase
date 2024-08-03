using SolarEase.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace SolarEase.Models.DTO.FAQCategoryDto
{
    public class CreateFAQCategoryDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public int Order { get; set; }

    }
}
