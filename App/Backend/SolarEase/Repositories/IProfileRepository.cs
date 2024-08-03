using SolarEase.Models.Domain;

namespace SolarEase.Repositories
{
    public interface IProfileRepository
    {
        Task<List<Profile>> GetAllAsync();
        Task<Profile?> GetByIdAsync(int id);
        Task<Profile> CreateAsync(Profile profileModel);
        Task<Profile?> DeleteAsync(int id);
        Task<Profile?> UpdateAsync(int id, Profile profileModel);
    }
}
