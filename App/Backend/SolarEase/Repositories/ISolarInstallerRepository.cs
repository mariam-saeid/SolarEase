using SolarEase.Models.Domain;

namespace SolarEase.Repositories
{
    public interface ISolarInstallerRepository
    {
        Task<List<SolarInstaller>> GetAllQueryAsync(String? nameQuery = null, String? cityQuery = null);
        Task<SolarInstaller?> GetByIdAsync(int id);
        Task<SolarInstaller> CreateAsync(SolarInstaller solarInstallerModel);
        Task<SolarInstaller?> UpdateAsync(int id, SolarInstaller solarInstallerModel);
        Task<SolarInstaller?> DeleteAsync(int id);
    }
}
