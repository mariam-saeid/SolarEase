using AutoMapper;
using SolarEase.Services;
using SolarEase.Mappings;
using SolarEase.Models.Domain;
using SolarEase.Models.DTO.AccountDto;
using SolarEase.Models.DTO.ProfileDto;
using SolarEase.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using SolarEase.Models.DTO.PersonDto;
using Microsoft.AspNetCore.Identity;
using SolarEase.Models.DTO.AdminDto;
using SolarEase.Models.DTO.CalculatorDto;

namespace SolarEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalculatorsController : ControllerBase
    {
        private readonly ICalculatorRepository calculatorRepository;
        private readonly CalculatorService calculatorService;
        private readonly IElectricityConsumptionRepository electricityTariffRepository;
        private readonly ISolarProductRepository solarProductRepository;
        private readonly IPersonRepository personRepository;
        private readonly IPeakHourRepository peakHourRepository;
        private readonly IMapper mapper;

        public CalculatorsController(ICalculatorRepository calculatorRepository,CalculatorService calculatorService,
            IElectricityConsumptionRepository electricityTariffRepository,ISolarProductRepository solarProductRepository,
            IPersonRepository personRepository,IPeakHourRepository peakHourRepository, IMapper mapper)
        {
            this.calculatorRepository = calculatorRepository;
            this.calculatorService = calculatorService;
            this.electricityTariffRepository = electricityTariffRepository;
            this.solarProductRepository = solarProductRepository;
            this.personRepository = personRepository;
            this.peakHourRepository = peakHourRepository;
            this.mapper = mapper;        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var calculatorsModel = await calculatorRepository.GetAllAsync();
            var calculatorsDto = calculatorsModel.CalculatorsInfo();
            return Ok(calculatorsDto);
        }

        //***************************************************************************************
        [HttpGet]
        [Route("Id/{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var calculatorModel = await calculatorRepository.GetByIdAsync(id);
            if (calculatorModel == null)
            {
                return NotFound();
            }
            var calculatorDto = calculatorModel.CalculatorInfo();
            return Ok(calculatorDto);
        }

        [HttpGet]
        [Route("personId/{personId:int}")]
        [Authorize]
        public async Task<IActionResult> GetByPersonId([FromRoute] int personId)
        {
            var personModel = await personRepository.GetByIdAsync(personId);
            if (personModel == null)
            {
                return NotFound("person not found");
            }
            var calculatorModel = await calculatorRepository.GetByPersonIdAsync(personId);
            if (calculatorModel == null)
            {
                return NotFound("calculator not found");
            }
            var calculatorDto = calculatorModel.CalculatorInfo();
            return Ok(calculatorDto);
        }

        //***************************************************************************************
        // create electricity bills dto
        [HttpPost]
        [Route("CreateByElectricityBills/{personId:int}")]
        [Authorize]
        public async Task<IActionResult> CreateByElectricityBills([FromRoute] int personId, [FromForm] ElectricityBillsDto electricityBillsDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //person
            var personModel = await personRepository.GetByIdAsync(personId);
            if (personModel == null)
                return NotFound("person not found");
            //peak hour
            var peakHourModel = await peakHourRepository.GetByCityAsync(personModel.City);
            if (peakHourModel == null)
                return NotFound("peak hour not found");

            //Monthly Consumption
            double monthlyConsumption = (electricityBillsDto.January + electricityBillsDto.February + electricityBillsDto.March +
            electricityBillsDto.April + electricityBillsDto.May + electricityBillsDto.June + electricityBillsDto.July
            + electricityBillsDto.August + electricityBillsDto.September + electricityBillsDto.October
                + electricityBillsDto.November + electricityBillsDto.December) / 12;

            //ElectricalCoverage
            double monthlyAvg = monthlyConsumption * (electricityBillsDto.ElectricalCoverage / 100);

            //dailyAvg
            double dailyConsumption = monthlyAvg / 30;

            //Calculator
            Calculator calculator = new Calculator();
            calculator.PersonId = personModel.Id;
            calculator.TotalCost = 0;

            //SystemSize
            calculator = calculatorService.CaluclateSystemSize(calculator, dailyConsumption, peakHourModel.PeakSunlightHour);

            //Panels
            calculator = await calculatorService.CaluclatePanelsAsync(calculator, solarProductRepository);

            //Inverter
            calculator = await calculatorService.CaluclateInverterAsync(calculator, solarProductRepository, electricityBillsDto.Devicesload, dailyConsumption, electricityBillsDto.Grid);

            //Battery
            if (!electricityBillsDto.Grid)
            {
                calculator = await calculatorService.CaluclateBatteryAsync(calculator, solarProductRepository);
            }
            else
            {
                //net meter
                calculator.TotalCost += 4000;

                //FinancialSavings
                calculator = await calculatorService.CaluclateFinancialSavingsAsync(calculator, electricityTariffRepository, peakHourModel.PeakSunlightHour, monthlyConsumption);

                //PaybackPeriod
                calculator = calculatorService.CaluclatePaybackPeriod(calculator);
            }

            // EnvironmentalBenefits
            calculator = calculatorService.CaluclateEnvironmentalBenefit(calculator);

            //delete all instances
            var calculators = await calculatorRepository.DeleteAllByPersonAsync(personId);

            if (calculators == null)
            {
                return NotFound("person not found");
            }

            // Create in database
            var calculatorModel = await calculatorRepository.CreateAsync(calculator);

            var calculatorDto = calculatorModel.CalculatorInfo();
            return Ok(calculatorDto);
        }

        // create max load dto
        [HttpPost]
        [Route("CreateByMaxLoad/{personId:int}")]
        [Authorize]
        public async Task<IActionResult> CreateByMaxLoad([FromRoute] int personId, [FromForm] MaxLoadDto maxLoadDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //person
            var personModel = await personRepository.GetByIdAsync(personId);
            if (personModel == null)
                return NotFound("person not found");
            //peak hour
            var peakHourModel = await peakHourRepository.GetByCityAsync(personModel.City);
            if (peakHourModel == null)
                return NotFound("peak hour not found");

            //Monthly Consumption
            double monthlyConsumption = maxLoadDto.MaxMonthLoad;

            //ElectricalCoverage
            double monthlyAvg = monthlyConsumption * (maxLoadDto.ElectricalCoverage / 100);

            //dailyAvg
            double dailyConsumption = monthlyAvg / 30;
            
            //Calculator
            Calculator calculator = new Calculator();
            calculator.PersonId = personModel.Id;
            calculator.TotalCost = 0;

            //SystemSize
            calculator = calculatorService.CaluclateSystemSize(calculator, dailyConsumption, peakHourModel.PeakSunlightHour);

            //Panels
            calculator = await calculatorService.CaluclatePanelsAsync(calculator, solarProductRepository);

            //Inverter
            calculator = await calculatorService.CaluclateInverterAsync(calculator, solarProductRepository, maxLoadDto.Devicesload, dailyConsumption, maxLoadDto.Grid);

            //Battery
            if (!maxLoadDto.Grid)
            {
                calculator = await calculatorService.CaluclateBatteryAsync(calculator, solarProductRepository);
            }
            else
            {
                //net meter
                calculator.TotalCost += 4000;

                //FinancialSavings
                calculator = await calculatorService.CaluclateFinancialSavingsAsync(calculator, electricityTariffRepository, peakHourModel.PeakSunlightHour, monthlyConsumption);

                //PaybackPeriod
                calculator = calculatorService.CaluclatePaybackPeriod(calculator);
            }

            // EnvironmentalBenefits
            calculator = calculatorService.CaluclateEnvironmentalBenefit(calculator);


            //delete all instances
            var calculators = await calculatorRepository.DeleteAllByPersonAsync(personId);

            if (calculators == null)
            {
                return NotFound("person not found");
            }

            // Create in database
            var calculatorModel = await calculatorRepository.CreateAsync(calculator);

            var calculatorDto = calculatorModel.CalculatorInfo();

            return Ok(calculatorDto);
        }

        //***************************************************************************************
        // delete
        [HttpDelete]
        [Route("Delete/{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var calculatorModel = await calculatorRepository.DeleteAsync(id);

            if (calculatorModel == null) { return NotFound(); }

            return Ok("Calculator Model Deleted Successfully");
        }
        // delete by personId
        [HttpDelete]
        [Route("DeleteAllByPersonId/{personId:int}")]
        [Authorize]
        public async Task<IActionResult> DeleteAllByPersonId([FromRoute] int personId)
        {
            var calculators = await calculatorRepository.DeleteAllByPersonAsync(personId);

            if (calculators == null) {
                return NotFound("person not found");
            }

            return Ok("Calculator Model Deleted Successfully");
        }

    }
}
