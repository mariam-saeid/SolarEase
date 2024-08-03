using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.ConstrainedExecution;
using static System.Net.Mime.MediaTypeNames;

namespace SolarEase.Models.Domain
{
    public class PeakHour
    {
        [Key]
        public int Id { get; set; }
        public string City { get; set; }
        public double PeakSunlightHour { get; set; }
    }
}
