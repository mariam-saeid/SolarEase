using System.ComponentModel.DataAnnotations;

namespace SolarEase.Models.DTO.CalculatorDto
{
    public class MaxLoadDto
    {
        [Required]
        public double MaxMonthLoad { get; set; }
        [Required]
        public bool Grid { get; set; }
        [Required]
        public double ElectricalCoverage { get; set; }
        [Required]
        public double Devicesload { get; set; }
    }
}
