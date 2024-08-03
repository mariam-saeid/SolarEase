using Microsoft.AspNetCore.Identity;
using SolarEase.Models.Domain;

namespace SolarEase.Repositories
{
    public interface IAccountRepository
    {

        Task<List<Account>> GetAllAsync();
        Task<Account?> GetByIdAsync(string id);
        Task<bool> CreateAsync(Account account, string password);
        Task<(IdentityResult, string?)> UpdateAsync(string Id, Account account, string newPassword);
        Task<bool> DeleteAsync(string userId);
        Task<IdentityResult> UpdateValidationAsync(string Id, Account account);
    }
}
