using System.ComponentModel.DataAnnotations;

namespace SolarEase.Models.DTO.SolarProductDto
{
    public class CreateSolarProductDto
    {
        [Required]
        public string CategoryName { get; set; }
        [Required]
        [Range(1, 10000000, ErrorMessage = "Price must be between 1 and 10000000.")]
        public double Price { get; set; }
        [Required]
        public IFormFile ImageUrl { get; set; }
        [Required]
        [MaxLength(30, ErrorMessage = "Brand cannot be over 30 over characters")]
        public string Brand { get; set; }
        [Required]
        [Range(1, 1000, ErrorMessage = "Capacity must be between 1 and 1000.")]
        public double Capacity { get; set; }
        [Required]
        public string MeasuringUnit { get; set; }
        [Range(1, 50, ErrorMessage = "Volt must be between 1 and 50.")]
        public double? Volt { get; set; }
    }
}
