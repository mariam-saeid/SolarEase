using SolarEase.Models.Domain;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SolarEase.Repositories
{
    public interface IAdminRepository
    {
        Task<List<Admin>> GetAllAsync();
        Task<Admin?> GetByIdAsync(int id);
        Task<Admin?> GetByEmailAsync(string Email);
        Task<Admin> CreateAsync(Admin adminModel);
        Task<Admin?> UpdateAsync(int id, Admin adminModel);
        Task<Admin?> DeleteAsync(int id);
    }
}
