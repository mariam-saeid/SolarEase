using System.ComponentModel.DataAnnotations;

namespace SolarEase.Models.DTO.CalculatorDto
{
    public class ElectricityBillsDto
    {
        [Required]
        public double January { get; set; }
        [Required]
        public double February { get; set; }
        [Required]
        public double March { get; set; }
        [Required]
        public double April { get; set; }
        [Required]
        public double May { get; set; }
        [Required]
        public double June { get; set; }
        [Required]
        public double July { get; set; }
        [Required]
        public double August { get; set; }
        [Required]
        public double September { get; set; }
        [Required]
        public double October { get; set; }
        [Required]
        public double November { get; set; }
        [Required]
        public double December { get; set; }
        [Required]
        public bool Grid { get; set; }
        [Required]
        public double ElectricalCoverage { get; set; }
        [Required]
        public double Devicesload { get; set; }

    }
}
