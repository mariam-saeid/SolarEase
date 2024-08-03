using SolarEase.Data;
using SolarEase.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace SolarEase.Repositories
{
    public class DailyAllSkyPredictionRepository : IDailyAllSkyPredictionRepository
    {
        private readonly SolarEaseAuthDbContext dbContext;
        public DailyAllSkyPredictionRepository(SolarEaseAuthDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        public async Task<List<DailyAllSkyPrediction>> GetAllAsync()
        {
            return await dbContext.DailyAllSkyPredictions.ToListAsync();
        }

        public async Task<List<DailyAllSkyPrediction>> GetLastFiveAsync(string City)
        {

            // Get the total count of records matching the conditions
            var totalCount = await dbContext.DailyAllSkyPredictions
                .Where(d => d.City == City)
                .CountAsync();

            // Calculate the skip count to retrieve the last five records
            var skipCount = totalCount - 5;
            skipCount = skipCount < 0 ? 0 : skipCount;

            // Fetch the last five records
            return await dbContext.DailyAllSkyPredictions
                .Where(d => d.City == City)
                .Skip(skipCount)
                .ToListAsync();

        }

        public async Task<DailyAllSkyPrediction?> GetLastByDayNumAsync(int DayNum, string City)
        {

            var count = await dbContext.DailyAllSkyPredictions
                .Where(i => i.DayNum == DayNum && i.City == City)
                .CountAsync();

            var lastRecord = await dbContext.DailyAllSkyPredictions
                .Where(i => i.DayNum == DayNum && i.City == City)
                .Skip(count - 1) // Skip all but the last record
                .FirstOrDefaultAsync();

            return lastRecord;

        }

        public async Task<DailyAllSkyPrediction?> GetByIdAsync(int id)
        {
            return await dbContext.DailyAllSkyPredictions.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<DailyAllSkyPrediction> CreateAsync(DailyAllSkyPrediction dailyEnergyPrediction)
        {
            await dbContext.DailyAllSkyPredictions.AddAsync(dailyEnergyPrediction);
            await dbContext.SaveChangesAsync();
            return dailyEnergyPrediction;
        }

        public async Task<DailyAllSkyPrediction?> DeleteAsync(int id)
        {
            var dailyEnergyPrediction = await dbContext.DailyAllSkyPredictions.FirstOrDefaultAsync(x => x.Id == id);
            if (dailyEnergyPrediction == null)
            {
                return null;
            }
            dbContext.DailyAllSkyPredictions.Remove(dailyEnergyPrediction);
            await dbContext.SaveChangesAsync();
            return dailyEnergyPrediction;
        }

        public async Task<List<DailyAllSkyPrediction>> DeleteAllAsync(string City)
        {
            var dailyEnergyPredictions = await dbContext.DailyAllSkyPredictions.Where(i => i.City == City).ToListAsync();
            if (dailyEnergyPredictions == null || dailyEnergyPredictions.Count == 0)
            {
                return null;
            }

            dbContext.DailyAllSkyPredictions.RemoveRange(dailyEnergyPredictions);
            await dbContext.SaveChangesAsync();
            return dailyEnergyPredictions;
        }

        public async Task<DailyAllSkyPrediction?> UpdateAsync(int id, DailyAllSkyPrediction dailyEnergyPrediction)
        {
            var existingDailyEnergyPrediction = await dbContext.DailyAllSkyPredictions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingDailyEnergyPrediction == null)
            {
                return null;
            }
            existingDailyEnergyPrediction.DayNum = dailyEnergyPrediction.DayNum;
            existingDailyEnergyPrediction.Date = dailyEnergyPrediction.Date;
            existingDailyEnergyPrediction.PredictedAllSky = dailyEnergyPrediction.PredictedAllSky;
            existingDailyEnergyPrediction.City=dailyEnergyPrediction.City;

            await dbContext.SaveChangesAsync();
            return existingDailyEnergyPrediction;
        }
    
    }
}
