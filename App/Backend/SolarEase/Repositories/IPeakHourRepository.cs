using SolarEase.Models.Domain;

namespace SolarEase.Repositories
{
    public interface IPeakHourRepository
    {
        Task<List<PeakHour>> GetAllAsync();
        Task<PeakHour?> GetByIdAsync(int id);
        Task<PeakHour?> GetByCityAsync(string city);
        Task<PeakHour> CreateAsync(PeakHour peakHour);
        Task<PeakHour?> DeleteAsync(int id);
        Task<PeakHour?> UpdateAsync(int id, PeakHour peakHour);
    }
}
