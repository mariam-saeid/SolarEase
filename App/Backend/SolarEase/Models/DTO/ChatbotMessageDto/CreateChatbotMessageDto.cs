using System.ComponentModel.DataAnnotations;

namespace SolarEase.Models.DTO.ChatbotMessageDto
{
    public class CreateChatbotMessageDto
    {
        [Required]
        public string Question { get; set; }

    }
}
