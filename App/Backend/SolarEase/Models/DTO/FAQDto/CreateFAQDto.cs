using System.ComponentModel.DataAnnotations;

namespace SolarEase.Models.DTO.FAQDto
{
    public class CreateFAQDto
    {
        [Required]
        public string Question { get; set; }
        [Required]
        public string Answer { get; set; }
        [Required]
        public bool Active { get; set; }
        [Required]
        public int Order { get; set; }
        [Required]
        public int FAQCategoryId { get; set; }
    }
}
