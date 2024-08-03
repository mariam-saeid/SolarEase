using SolarEase.Models.Domain;

namespace SolarEase.Repositories
{
    public interface IFAQCategoryRepository
    {
        Task<List<FAQCategory>> GetAllAsync();
        Task<FAQCategory?> GetByIdAsync(int id);
        Task<FAQCategory?> GetByNameAsync(string name);
        Task<FAQCategory> CreateAsync(FAQCategory fAQCategory);
        Task<FAQCategory?> DeleteAsync(int id);
        Task<FAQCategory?> UpdateAsync(int id, FAQCategory fAQCategory);
    }
}
