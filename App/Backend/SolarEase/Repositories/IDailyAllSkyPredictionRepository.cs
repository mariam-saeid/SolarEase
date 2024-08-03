using SolarEase.Models.Domain;

namespace SolarEase.Repositories
{
    public interface IDailyAllSkyPredictionRepository
    {
        Task<List<DailyAllSkyPrediction>> GetAllAsync();
        Task<List<DailyAllSkyPrediction>> GetLastFiveAsync(string City);
        Task<DailyAllSkyPrediction?> GetLastByDayNumAsync(int DayNum, string City);
        Task<DailyAllSkyPrediction?> GetByIdAsync(int id);
        Task<DailyAllSkyPrediction> CreateAsync(DailyAllSkyPrediction dailyEnergyPrediction);
        Task<DailyAllSkyPrediction?> DeleteAsync(int id);
        Task<List<DailyAllSkyPrediction>> DeleteAllAsync(string City);
        Task<DailyAllSkyPrediction?> UpdateAsync(int id, DailyAllSkyPrediction dailyEnergyPrediction);
    }
}
