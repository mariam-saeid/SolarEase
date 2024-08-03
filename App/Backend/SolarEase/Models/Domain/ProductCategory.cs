using System.ComponentModel.DataAnnotations;

namespace SolarEase.Models.Domain
{
    public class ProductCategory
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<SolarProduct> SolarProducts { get; set; }
    }
}
