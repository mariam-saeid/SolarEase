using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace SolarEase.Models.DTO.PeakHourDto
{
    public class PeakHourDto
    {
        public int Id { get; set; }
        public string City { get; set; }
        public double PeakSunlightHour { get; set; }
    }
}
