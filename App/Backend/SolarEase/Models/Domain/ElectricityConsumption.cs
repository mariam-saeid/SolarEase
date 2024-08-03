using System.ComponentModel.DataAnnotations;

namespace SolarEase.Models.Domain
{
    public class ElectricityConsumption
    {
        [Key]
        public int Id { get; set; }
        public int StartRange { get; set; }
        public int EndRange { get; set; }
        public ICollection<ElectricityConsumptionSubrange> ElectricityConsumptionSubranges { get; set; }

    }
}
