using Microsoft.EntityFrameworkCore;
using SolarEase.Data;

namespace SolarEase.Repositories
{
    public class ElectricityConsumptionRepository : IElectricityConsumptionRepository
    {
        private readonly SolarEaseAuthDbContext dbContext;
        public ElectricityConsumptionRepository(SolarEaseAuthDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<double> CalculateElectricityCostAsync(double consumption)
        {
            // Initialize the total cost variable
            double totalCost = 0.0;

            // Retrieve the consumption range from the database that matches the user's consumption
            var range = await dbContext.ElectricityConsumptions.Include(r => r.ElectricityConsumptionSubranges)
        .FirstOrDefaultAsync(r => r.StartRange <= consumption && r.EndRange >= consumption);

            // If a valid range is found
            if (range != null)
            {
                // Iterate through each subrange within the selected range
                foreach (var subrange in range.ElectricityConsumptionSubranges)
                {
                    // Calculate the consumption within the current subrange
                    double subrangeConsumption = Math.Min(subrange.EndRange - subrange.StartRange + 1, consumption);

                    // Add the cost of the consumed units in the current subrange to the total cost
                    totalCost += subrangeConsumption * subrange.Price;

                    // Deduct the consumed units from the total consumption
                    consumption -= subrangeConsumption;

                    // If the user's consumption becomes zero or negative, exit the loop
                    if (consumption <= 0)
                        break;
                }
            }

            // Return the total cost of consumption
            return totalCost;
        }
    }
}
