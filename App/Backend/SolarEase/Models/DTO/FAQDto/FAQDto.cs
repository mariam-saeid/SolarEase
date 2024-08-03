using SolarEase.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarEase.Models.DTO.FAQDto
{
    public class FAQDto
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public bool Active { get; set; }
        public int Order { get; set; }
        public int FAQCategoryId { get; set; }
        public string FAQCategoryName { get; set; }

    }
}
