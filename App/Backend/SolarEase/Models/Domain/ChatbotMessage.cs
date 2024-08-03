using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarEase.Models.Domain
{
    public class ChatbotMessage
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }

        [ForeignKey(nameof(Person))]
        public int PersonId { get; set; }
        public virtual Person Person { get; set; }
    }
}
