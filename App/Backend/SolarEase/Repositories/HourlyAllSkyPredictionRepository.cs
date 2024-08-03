using SolarEase.Data;
using SolarEase.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace SolarEase.Repositories
{
    public class HourlyAllSkyPredictionRepository : IHourlyAllSkyPredictionRepository
    {
        private readonly SolarEaseAuthDbContext dbContext;
        public HourlyAllSkyPredictionRepository(SolarEaseAuthDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<HourlyAllSkyPrediction>> GetAllAsync()
        {
            return await dbContext.HourlyAllSkyPredictions.ToListAsync();
        }

        public async Task<List<HourlyAllSkyPrediction>> GetByCityAsync(string City)
        {
            return await dbContext.HourlyAllSkyPredictions.Where(i => i.City == City).ToListAsync();
        }

        public async Task<HourlyAllSkyPrediction?> GetByIdAsync(int id)
        {
            return await dbContext.HourlyAllSkyPredictions.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<List<HourlyAllSkyPrediction>> GetByDayNumAsync(int DayNum, string City)
        {
            return await dbContext.HourlyAllSkyPredictions.Where(i => i.DayNum == DayNum && i.City == City).ToListAsync();
        }

        public async Task<HourlyAllSkyPrediction> CreateAsync(HourlyAllSkyPrediction hourlyEnergyPrediction)
        {
            await dbContext.HourlyAllSkyPredictions.AddAsync(hourlyEnergyPrediction);
            await dbContext.SaveChangesAsync();
            return hourlyEnergyPrediction;
        }

        public async Task<HourlyAllSkyPrediction?> DeleteAsync(int id)
        {
            var hourlyAllSkyPrediction = await dbContext.HourlyAllSkyPredictions.FirstOrDefaultAsync(x => x.Id == id);
            if (hourlyAllSkyPrediction == null)
            {
                return null;
            }
            dbContext.HourlyAllSkyPredictions.Remove(hourlyAllSkyPrediction);
            await dbContext.SaveChangesAsync();
            return hourlyAllSkyPrediction;
        }

        public async Task<List<HourlyAllSkyPrediction>> DeleteAllAsync(string City)
        {
            var hourlyAllSkyPredictions = await dbContext.HourlyAllSkyPredictions.Where(i => i.City == City).ToListAsync();
            if (hourlyAllSkyPredictions == null || hourlyAllSkyPredictions.Count == 0)
            {
                return null;
            }

            dbContext.HourlyAllSkyPredictions.RemoveRange(hourlyAllSkyPredictions);
            await dbContext.SaveChangesAsync();
            return hourlyAllSkyPredictions;
        }

        public async Task<HourlyAllSkyPrediction?> UpdateAsync(int id, HourlyAllSkyPrediction hourlyAllSkyPrediction)
        {
            var existingHourlyAllSkyPrediction = await dbContext.HourlyAllSkyPredictions.FirstOrDefaultAsync(x => x.Id == id);
            if (existingHourlyAllSkyPrediction == null)
            {
                return null;
            }
            existingHourlyAllSkyPrediction.City = hourlyAllSkyPrediction.City;
            existingHourlyAllSkyPrediction.DayNum = hourlyAllSkyPrediction.DayNum;
            existingHourlyAllSkyPrediction.Date = hourlyAllSkyPrediction.Date;
            existingHourlyAllSkyPrediction.Hour = hourlyAllSkyPrediction.Hour;
            existingHourlyAllSkyPrediction.PredictedAllSky = hourlyAllSkyPrediction.PredictedAllSky;

            await dbContext.SaveChangesAsync();
            return existingHourlyAllSkyPrediction;
        }

    }
}
