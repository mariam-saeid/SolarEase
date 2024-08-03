using SolarEase.Data;
using SolarEase.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace SolarEase.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly SolarEaseAuthDbContext _context;
        public ProductCategoryRepository(SolarEaseAuthDbContext dbontext)
        {
            this._context = dbontext;
        }

        public async Task<List<ProductCategory>> GetAllAsync()
        {
            return await _context.ProductCategories.ToListAsync();
        }

        public async Task<ProductCategory?> GetByIdAsync(int id)
        {
            return await _context.ProductCategories.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<ProductCategory?> GetByNameAsync(string Name)
        {
            return await _context.ProductCategories.FirstOrDefaultAsync(i => i.Name == Name);
        }

        public async Task<ProductCategory> CreateAsync(ProductCategory productCategory)
        {
            _context.ProductCategories.Add(productCategory);
            await _context.SaveChangesAsync();

            return productCategory;
        }

        public async Task<ProductCategory?> DeleteAsync(int id)
        {
            var productCategory = await _context.ProductCategories.FirstOrDefaultAsync(x => x.Id == id);
            if (productCategory == null)
            {
                return null;
            }
            _context.ProductCategories.Remove(productCategory);
            await _context.SaveChangesAsync();
            return productCategory;
        }

        public async Task<ProductCategory?> UpdateAsync(int id, ProductCategory productCategory)
        {
            var existingProductCategory = await _context.ProductCategories.FindAsync(id);
            if (existingProductCategory == null)
            {
                return null;
            }
            existingProductCategory.Name = productCategory.Name;
            await _context.SaveChangesAsync();
            return existingProductCategory;
        }
    }
}
