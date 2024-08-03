using SolarEase.Models.Domain;

namespace SolarEase.Repositories
{
    public interface IHourlyAllSkyPredictionRepository
    {
        Task<List<HourlyAllSkyPrediction>> GetAllAsync();
        Task<List<HourlyAllSkyPrediction>> GetByCityAsync(string City);
        Task<HourlyAllSkyPrediction?> GetByIdAsync(int id);
        Task<List<HourlyAllSkyPrediction>> GetByDayNumAsync(int DayNum, string City);
        Task<HourlyAllSkyPrediction> CreateAsync(HourlyAllSkyPrediction hourlyAllSkyPrediction);
        Task<HourlyAllSkyPrediction?> DeleteAsync(int id);
        Task<List<HourlyAllSkyPrediction>> DeleteAllAsync(string City);
        Task<HourlyAllSkyPrediction?> UpdateAsync(int id, HourlyAllSkyPrediction hourlyAllSkyPrediction);
    }
}
