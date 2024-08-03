using SolarEase.Data;
using SolarEase.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace SolarEase.Repositories
{
    public class CalculatorRepository : ICalculatorRepository
    {
        private readonly SolarEaseAuthDbContext dbContext;
        public CalculatorRepository(SolarEaseAuthDbContext dbontext)
        {
            this.dbContext = dbontext;
        }

        public async Task<List<Calculator>> GetAllAsync()
        {
            return await dbContext.Calculators
                .Include(c => c.Person)
                .ToListAsync();
        }

        public async Task<Calculator?> GetByIdAsync(int id)
        {
            return await dbContext.Calculators
                .Include(c => c.Person)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Calculator?> GetByPersonIdAsync(int personId)
        {
            return await dbContext.Calculators
                .Include(c => c.Person)
                .FirstOrDefaultAsync(c => c.PersonId == personId);
        }

        public async Task<Calculator> CreateAsync(Calculator calculatorModel)
        {
            await dbContext.Calculators.AddAsync(calculatorModel);
            await dbContext.SaveChangesAsync();
            return calculatorModel;
        }

        public async Task<Calculator?> DeleteAsync(int id)
        {
            var calculatorModel = await dbContext.Calculators.FirstOrDefaultAsync(x => x.Id == id);
            if (calculatorModel == null)
            {
                return null;
            }
            dbContext.Calculators.Remove(calculatorModel);
            await dbContext.SaveChangesAsync();
            return calculatorModel;
        }

        public async Task<List<Calculator>?> DeleteAllByPersonAsync(int personId)
        {
            if (!await dbContext.Persons.AnyAsync(p => p.Id == personId))
                return null;

            var calculators = await dbContext.Calculators.Where(c => c.PersonId == personId).ToListAsync();

            dbContext.Calculators.RemoveRange(calculators);
            await dbContext.SaveChangesAsync();
            return calculators;
        }

    }
}
