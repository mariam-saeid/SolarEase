namespace SolarEase.Models.DTO.EnergyPredictionDto
{
    public class HourlyEnergyPredictionDto
    {
        public int Id { get; set; }
        public string City { get; set; }
        public int DayNum { get; set; }
        public string Hour { get; set; }
        public String Date { get; set; }
        public double PredictedEnergy { get; set; }
        public string PredictedEnergyStr { get; set; }

    }
}
