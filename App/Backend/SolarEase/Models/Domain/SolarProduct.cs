using SolarEase.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarEase.Models.Domain
{
    public class SolarProduct
    {
        [Key]
        public int Id { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }
        public string Brand { get; set; }
        public double Capacity { get; set; }
        public string MeasuringUnit { get; set; }
        public double? Volt { get; set; }
        public bool IsProductPost { get; set; }

        [ForeignKey(nameof(ProductCategory))]
        public int ProductCategoryId { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
        public virtual ICollection<FavoriteProduct> FavoriteProducts { get; set; }
    }
}
