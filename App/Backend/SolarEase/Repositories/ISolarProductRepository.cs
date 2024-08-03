using SolarEase.Models.Domain;

namespace SolarEase.Repositories
{
    public interface ISolarProductRepository
    {
        Task<List<SolarProduct>> GetAllAsync(String? query);
        Task<SolarProduct?> GetByIdAsync(int id);
        Task<List<SolarProduct>> GetByCategoryNameAsync(string categoryName);
        Task<List<SolarProduct>> GetAllCapacitySortedAsync(string categoryName, double minCapacity, double maxCapacity);
        Task<double> GetNextHigherCapacityAsync(string categoryName, double capacity, string? brandName);
        Task<double> GetPriceByCapacityAsync(double capacity, string categoryName, string? brandName);
        Task<SolarProduct> CreateAsync(SolarProduct solarProductModel);
        Task<SolarProduct?> DeleteAsync(int id);
        Task<SolarProduct?> UpdateAsync(int id, SolarProduct solarProductModel);

    }
}
