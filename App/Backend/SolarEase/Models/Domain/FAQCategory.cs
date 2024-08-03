using System.ComponentModel.DataAnnotations;

namespace SolarEase.Models.Domain
{
    public class FAQCategory
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public virtual List<FAQ> FAQs { get; set; }
    }
}
