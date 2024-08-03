using AutoMapper;
using SolarEase.Services;
using SolarEase.Mappings;
using SolarEase.Models.Domain;
using SolarEase.Models.DTO.AccountDto;
using SolarEase.Models.DTO.ProfileDto;
using SolarEase.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;
using SolarEase.Models.DTO.AdminDto;
using Microsoft.EntityFrameworkCore;

namespace SolarEase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ElectricityConsumptionController : ControllerBase
    {
        private readonly IElectricityConsumptionRepository electricityConsumptionRepository;

        public ElectricityConsumptionController(IElectricityConsumptionRepository electricityConsumptionRepository)
        {
            this.electricityConsumptionRepository = electricityConsumptionRepository;
        }

        [HttpGet("CalculateElectricityCost")]
        public async Task<IActionResult> CalculateElectricityCost(double consumption)
        {
            // Call the CalculateConsumptionCost method to calculate the cost based on the consumption
            double cost = await electricityConsumptionRepository.CalculateElectricityCostAsync(consumption);

            // Return the calculated cost
            return Ok(cost);
        }

    }
}
