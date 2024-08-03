using SolarEase.Models.Domain;

namespace SolarEase.Repositories
{
    public interface IFAQRepository
    {
        Task<List<FAQ>> GetAllAsync();
        Task<FAQ?> GetByIdAsync(int id);
        Task<List<FAQ>> GetByCategoryNameAsync(string fAQCategory);
        Task<List<FAQ>> GetByCategoryIdAsync(int fAQCategoryId);
        Task<FAQ> CreateAsync(FAQ fAQ);
        Task<FAQ?> DeleteAsync(int id);
        Task<FAQ?> UpdateAsync(int id, FAQ fAQ);
    }
}
