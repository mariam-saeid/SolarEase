using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarEase.Models.Domain
{
    public class FAQ
    {
        [Key]
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool Active { get; set; }
        public int Order { get; set; }

        [ForeignKey(nameof(FAQCategory))]
        public int FAQCategoryId { get; set; }
        public virtual FAQCategory FAQCategory { get; set; }
    }
}
