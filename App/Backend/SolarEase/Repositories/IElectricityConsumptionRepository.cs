using Microsoft.AspNetCore.Identity;
using SolarEase.Models.Domain;

namespace SolarEase.Repositories
{
    public interface IElectricityConsumptionRepository
    {
        Task<double> CalculateElectricityCostAsync(double consumption);
    }
}
