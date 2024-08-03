using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarEase.Models.Domain
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public DateTime SentDate { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        [ForeignKey(nameof(Person))]
        public int PersonId { get; set; }
        public virtual Person Person { get; set; }
    }
}
