using SolarEase.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace SolarEase.Models.DTO.ProductCategoryDto
{
    public class CreateProductCategoryDto
    {
        [Required]
        public string Name { get; set; }
       
    }
}
