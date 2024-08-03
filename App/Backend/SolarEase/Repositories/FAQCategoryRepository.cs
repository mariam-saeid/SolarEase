using SolarEase.Data;
using SolarEase.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace SolarEase.Repositories
{
    public class FAQCategoryRepository : IFAQCategoryRepository
    {
        private readonly SolarEaseAuthDbContext _context;
        public FAQCategoryRepository(SolarEaseAuthDbContext dbontext)
        {
            this._context = dbontext;
        }

        public async Task<List<FAQCategory>> GetAllAsync()
        {
            return await _context.FAQCategories.OrderBy(p => p.Order).ToListAsync();
        }

        public async Task<FAQCategory?> GetByIdAsync(int id)
        {
            return await _context.FAQCategories.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<FAQCategory?> GetByNameAsync(string Name)
        {
            return await _context.FAQCategories.FirstOrDefaultAsync(i => i.Name == Name);
        }

        public async Task<FAQCategory> CreateAsync(FAQCategory fAQCategory)
        {
            _context.FAQCategories.Add(fAQCategory);
            await _context.SaveChangesAsync();

            return fAQCategory;
        }

        public async Task<FAQCategory?> DeleteAsync(int id)
        {
            var fAQCategory = await _context.FAQCategories.FirstOrDefaultAsync(x => x.Id == id);
            if (fAQCategory == null)
            {
                return null;
            }
            _context.FAQCategories.Remove(fAQCategory);
            await _context.SaveChangesAsync();
            return fAQCategory;
        }

        public async Task<FAQCategory?> UpdateAsync(int id, FAQCategory fAQCategory)
        {
            var existingFAQCategory = await _context.FAQCategories.FindAsync(id);
            if (existingFAQCategory == null)
            {
                return null;
            }
            existingFAQCategory.Name = fAQCategory.Name;
            existingFAQCategory.Order = fAQCategory.Order;
            await _context.SaveChangesAsync();
            return existingFAQCategory;
        }
    }
}
