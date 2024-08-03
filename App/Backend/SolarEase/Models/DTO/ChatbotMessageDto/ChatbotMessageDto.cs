using SolarEase.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarEase.Models.DTO.ChatbotMessageDto
{
    public class ChatbotMessageDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public int PersonId { get; set; }
        public string? ImageUrl { get; set; }
    }
}
