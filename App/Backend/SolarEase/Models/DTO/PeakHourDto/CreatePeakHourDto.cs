using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace SolarEase.Models.DTO.PeakHourDto
{
    public class CreatePeakHourDto
    {
        [Required]
        public string City { get; set; }
        [Required]
        public double YearlyInPlaneIrradiation { get; set; }
    }
}
