using SolarEase.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SolarEase.Models.DTO.CalculatorDto
{
    public class CalculatorDto
    {
        public int Id { get; set; }
        //SystemSize
        public string SystemSize { get; set; }
        public string RoofSpace { get; set; }
        //Panels
        public string PanelsCapacity { get; set; }
        public string NumofPanels { get; set; }
        public string PanelsPrice { get; set; }
        //Inverter
        public string InverterCapacity { get; set; }
        public string InverterPrice { get; set; }
        //Battery
        public string? BatteryCapacity { get; set; }
        public string? NumofBatteries { get; set; }
        public string? BatteryPrice { get; set; }
        //TotalCost
        public string TotalCost { get; set; }
        //FinancialSavings
        public string? FinancialSavingMonthly { get; set; }
        public string? FinancialSavingYearly { get; set; }
        public string? FinancialSavingTwentyFiveYear { get; set; }
        //PaybackPeriod
        public string? PaybackPeriod { get; set; }
        //EnvironmentalBenefit
        public string EnvironmentalBenefitMonthly { get; set; }
        public string EnvironmentalBenefitYearly { get; set; }
        public string EnvironmentalBenefitTwentyFiveYear { get; set; }
        //Person
        public int PersonId { get; set; }
    }
}
