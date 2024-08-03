using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SolarEase.Models.Domain
{
    public class ElectricityConsumptionSubrange
    {
        [Key]
        public int Id { get; set; }
        public int StartRange { get; set; }
        public int EndRange { get; set; }
        public double Price { get; set; }
        [ForeignKey(nameof(ElectricityConsumption))]
        public int ElectricityConsumptionId { get; set; }
        public ElectricityConsumption ElectricityConsumption { get; set; }

    }
}
