using SolarEase.Models.Domain;

namespace SolarEase.Repositories
{
    public interface IPersonRepository
    {
        Task<List<Person>> GetAllAsync();
        Task<Person?> GetByIdAsync(int id);
        Task<Person?> GetByEmailAsync(string Email);
        Task<Person> CreateAsync(Person personModel);
        Task<Person?> UpdateAllAsync(int id, Person personModel);
        Task<Person?> DeleteAsync(int id);
    }
}
