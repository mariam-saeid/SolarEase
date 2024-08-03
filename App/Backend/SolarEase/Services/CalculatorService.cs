using SolarEase.Models.Domain;
using Microsoft.AspNetCore.Hosting;
using SolarEase.Models.DTO.CalculatorDto;
using SolarEase.Repositories;
using System.Text.Json.Nodes;

namespace SolarEase.Services
{
    public class CalculatorService
    {
        public CalculatorService(){}

        public Calculator CaluclateSystemSize(Calculator calculator,double dailyAvg, double peakSunlightHour)
        {
            double systemSize = (dailyAvg / peakSunlightHour) * 1.3;

            calculator.SystemSize = Math.Round(systemSize,2);
            calculator.RoofSpace = Math.Round(systemSize*10,2);

            return calculator;
        }

        public async Task<Calculator> CaluclatePanelsAsync(Calculator calculator,ISolarProductRepository solarProductRepository)
        {
            //Panels Capacity
            var categoryName = "Solar Panel";
            var solarProductsModel = await solarProductRepository.GetAllCapacitySortedAsync(categoryName,300,420);
            calculator.MiniPanelsCapacity = solarProductsModel[0].Capacity;
            calculator.MaxPanelsCapacity = solarProductsModel[(solarProductsModel.Count - 1)].Capacity;
            
            //Panels Price
            var miniPanelCapacityPrice = await solarProductRepository.GetPriceByCapacityAsync(calculator.MiniPanelsCapacity, categoryName, "");
            var maxiPanelCapacityPrice = await solarProductRepository.GetPriceByCapacityAsync(calculator.MaxPanelsCapacity, categoryName, "");

            //Panels Num
            calculator.MiniNumofPanels = (int)Math.Ceiling((calculator.SystemSize * 1000) / calculator.MiniPanelsCapacity);
            calculator.MaxNumofPanels = (int)Math.Ceiling((calculator.SystemSize * 1000) / calculator.MaxPanelsCapacity);

            //Total Price
            calculator.MiniPanelsPrice = Math.Round(calculator.MiniPanelsCapacity * miniPanelCapacityPrice * calculator.MiniNumofPanels, 2);
            calculator.MaxPanelsPrice = Math.Round(calculator.MaxPanelsCapacity * maxiPanelCapacityPrice * calculator.MaxNumofPanels, 2);

            calculator.TotalCost += calculator.MaxPanelsPrice;

            return calculator;
        }

        public async Task<Calculator> CaluclateInverterAsync(Calculator calculator, ISolarProductRepository solarProductRepository, double Devicesload, double dailyAvg, bool grid)
        {
            double weightage;
            weightage = Devicesload * 1.3;
            weightage /= 1000;

            var categoryName = "Inverter";
            string gridStr;
            if (grid)
                gridStr = "On-Grid";
            else
                gridStr = "Off-Grid";

            calculator.InverterCapacity = await solarProductRepository.GetNextHigherCapacityAsync(categoryName, weightage, gridStr);
            calculator.InverterPrice = await solarProductRepository.GetPriceByCapacityAsync(calculator.InverterCapacity, categoryName, gridStr);

            calculator.TotalCost += calculator.InverterPrice;


            return calculator;
        }

        public async Task<Calculator> CaluclateBatteryAsync(Calculator calculator, ISolarProductRepository solarProductRepository)
        {
            double halfSystemSize = (calculator.SystemSize/2)*1.3;
            
            var categoryName = "Battery";
            calculator.BatteryCapacity = 200;
            double capacityPrice = await solarProductRepository.GetPriceByCapacityAsync((double)calculator.BatteryCapacity, categoryName, "");
            
            double totalCapacity = (double)(calculator.BatteryCapacity * 12);
            calculator.NumofBatteries = (int)Math.Ceiling((halfSystemSize * 1000) / totalCapacity);
            calculator.BatteryPrice = (int)calculator.NumofBatteries * capacityPrice;

            calculator.TotalCost += (double)calculator.BatteryPrice;

            return calculator;
        }

        public async Task<Calculator> CaluclateFinancialSavingsAsync(Calculator calculator,IElectricityConsumptionRepository electricityTariffRepository,double peakSunlightHour, double oldMonthlyConsumption)
        {
            double newMonthlyConsumption = calculator.SystemSize*peakSunlightHour*30;
            double diff = oldMonthlyConsumption - newMonthlyConsumption;
            if (diff < 0) { diff = 0; }

            double oldCost = await electricityTariffRepository.CalculateElectricityCostAsync(oldMonthlyConsumption);
            double newCost = await electricityTariffRepository.CalculateElectricityCostAsync(diff);

            double saved = oldCost - newCost;

            calculator.FinancialSavingMonthly = saved;
            calculator.FinancialSavingYearly = saved * 12;
            calculator.FinancialSavingTwentyFiveYear = saved * 12 * 25;
            
            return calculator;
        }

        public Calculator CaluclatePaybackPeriod(Calculator calculator)
        {
            calculator.PaybackPeriod = Math.Round(calculator.TotalCost / (double)calculator.FinancialSavingYearly, 2);
            return calculator;
        }

        public Calculator CaluclateEnvironmentalBenefit(Calculator calculator)
        {
            calculator.EnvironmentalBenefitMonthly = Math.Round((calculator.SystemSize * 0.475) * 30,2);
            calculator.EnvironmentalBenefitYearly = Math.Round((calculator.SystemSize * 0.475) * 365,2);
            calculator.EnvironmentalBenefitTwentyFiveYear = Math.Round((calculator.SystemSize * 0.475) * 365 * 25,2);

            return calculator;
        }

    }
}
