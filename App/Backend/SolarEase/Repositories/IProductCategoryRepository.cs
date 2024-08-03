using SolarEase.Models.Domain;

namespace SolarEase.Repositories
{
    public interface IProductCategoryRepository
    {
        Task<List<ProductCategory>> GetAllAsync();
        Task<ProductCategory?> GetByIdAsync(int id);
        Task<ProductCategory?> GetByNameAsync(string name);
        Task<ProductCategory> CreateAsync(ProductCategory productCategory);
        Task<ProductCategory?> DeleteAsync(int id);
        Task<ProductCategory?> UpdateAsync(int id, ProductCategory productCategory);
    }
}
