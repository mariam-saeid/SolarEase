using System.ComponentModel.DataAnnotations;

namespace SolarEase.Models.Domain
{
    public class HourlyAllSkyPrediction
    {
        [Key]
        public int Id { get; set; }
        public string City { get; set; }
        public int DayNum { get; set; }
        public string Hour { get; set; }
        public String Date { get; set; }
        public double PredictedAllSky { get; set; }

    }
}
