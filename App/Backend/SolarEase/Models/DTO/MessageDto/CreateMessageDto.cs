using System.ComponentModel.DataAnnotations;

namespace SolarEase.Models.DTO.MessageDto
{
    public class CreateMessageDto
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Body { get; set; }
    }
}
