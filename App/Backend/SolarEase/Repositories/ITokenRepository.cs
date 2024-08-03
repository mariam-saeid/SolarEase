using SolarEase.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace SolarEase.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(Account account, List<string> roles);
    }
}
