using SolarEase.Data;
using SolarEase.Models.Domain;
using Microsoft.EntityFrameworkCore;
using SolarEase.Services;
using System.Net.Http;
using SolarEase.Models.DTO.FAQDto;

namespace SolarEase.Repositories
{
    public class FAQRepository : IFAQRepository
    {
        private readonly SolarEaseAuthDbContext dbContext;

        public FAQRepository(SolarEaseAuthDbContext dbontext)
        {
            this.dbContext = dbontext;
        }

        public async Task<List<FAQ>> GetAllAsync()
        {
            return await dbContext.FAQs.Include(p => p.FAQCategory).OrderBy(p => p.Order).ToListAsync();
            
        }

        public async Task<List<FAQ>> GetByCategoryNameAsync(string fAQCategory)
        {
            return await dbContext.FAQs
                .Include(p => p.FAQCategory)
                .Where(p => p.FAQCategory.Name == fAQCategory && p.Active).OrderBy(p => p.Order).ToListAsync();
        }

        public async Task<List<FAQ>> GetByCategoryIdAsync(int fAQCategoryId)
        {
            return await dbContext.FAQs
                .Include(p => p.FAQCategory)
                .Where(p => p.FAQCategory.Id == fAQCategoryId && p.Active).OrderBy(p => p.Order).ToListAsync();
        }

        public async Task<FAQ?> GetByIdAsync(int id)
        {
            return await dbContext.FAQs.Include(p => p.FAQCategory).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<FAQ> CreateAsync(FAQ fAQ)
        {
            await dbContext.FAQs.AddAsync(fAQ);
            await dbContext.SaveChangesAsync();
            return fAQ;
        }
       
        public async Task<FAQ?> DeleteAsync(int id)
        {
            var fAQ = await dbContext.FAQs.Include(p => p.FAQCategory).FirstOrDefaultAsync(x => x.Id == id);
            if (fAQ == null)
            {
                return null;
            }
            dbContext.FAQs.Remove(fAQ);
            await dbContext.SaveChangesAsync();
            return fAQ;
        }

        public async Task<FAQ?> UpdateAsync(int id, FAQ fAQ)
        {
            var existingFAQ = await dbContext.FAQs.Include(p => p.FAQCategory).FirstOrDefaultAsync(x => x.Id == id);
            if (existingFAQ == null)
            {
                return null;
            }
           
            existingFAQ.Question= fAQ.Question;
            existingFAQ.Answer = fAQ.Answer;
            existingFAQ.Active = fAQ.Active;
            existingFAQ.Order = fAQ.Order;
            existingFAQ.FAQCategoryId= fAQ.FAQCategoryId;

            await dbContext.SaveChangesAsync();
            return existingFAQ;
        }


    }
}
