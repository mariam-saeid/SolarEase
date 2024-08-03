using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarEase.Models.Domain
{
    public class Calculator
    {
        [Key]
        public int Id { get; set; }
        //SystemSize
        public double SystemSize { get; set; }
        public double RoofSpace { get; set; }
        //Panels
        public double MiniPanelsCapacity { get; set; }
        public int MiniNumofPanels { get; set; }
        public double MiniPanelsPrice { get; set; }
        public double MaxPanelsCapacity { get; set; }
        public int MaxNumofPanels { get; set; }
        public double MaxPanelsPrice { get; set; }
        //Inverter
        public double InverterCapacity { get; set; }
        public double InverterPrice { get; set; }
        //Battery
        public double? BatteryCapacity { get; set; }
        public int? NumofBatteries { get; set; }
        public double? BatteryPrice { get; set; }
        //TotalCost
        public double TotalCost { get; set; }
        //FinancialSavings
        public double? FinancialSavingMonthly { get; set; }
        public double? FinancialSavingYearly { get; set; }
        public double? FinancialSavingTwentyFiveYear { get; set; }
        //PaybackPeriod
        public double? PaybackPeriod { get; set; }
        //EnvironmentalBenefit
        public double EnvironmentalBenefitMonthly { get; set; }
        public double EnvironmentalBenefitYearly { get; set; }
        public double EnvironmentalBenefitTwentyFiveYear { get; set; }
        //Person
        [ForeignKey(nameof(Person))]
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}
