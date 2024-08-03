using SolarEase.Data;
using SolarEase.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace SolarEase.Repositories
{
    public class PeakHourRepository : IPeakHourRepository
    {
        private readonly SolarEaseAuthDbContext dbContext;
        public PeakHourRepository(SolarEaseAuthDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        
        public async Task<List<PeakHour>> GetAllAsync()
        {
            return await dbContext.PeakHours.ToListAsync();
        }
        
        public async Task<PeakHour?> GetByIdAsync(int id)
        {
            return await dbContext.PeakHours.FirstOrDefaultAsync(i => i.Id == id);
        }
        
        public async Task<PeakHour?> GetByCityAsync(string city)
        {
            return await dbContext.PeakHours.FirstOrDefaultAsync(i => i.City == city);
        }
        
        public async Task<PeakHour> CreateAsync(PeakHour peakHour)
        {
            await dbContext.PeakHours.AddAsync(peakHour);
            await dbContext.SaveChangesAsync();
            return peakHour;
        }
        
        public async Task<PeakHour?> DeleteAsync(int id)
        {
            var peakHour = await dbContext.PeakHours.FirstOrDefaultAsync(x => x.Id == id);
            if (peakHour == null)
            {
                return null;
            }
            dbContext.PeakHours.Remove(peakHour);
            await dbContext.SaveChangesAsync();
            return peakHour;
        }
        
        public async Task<PeakHour?> UpdateAsync(int id, PeakHour peakHour)
        {
            var existingPeakHour = await dbContext.PeakHours.FirstOrDefaultAsync(x => x.Id == id);
            if (existingPeakHour == null)
            {
                return null;
            }
            existingPeakHour.City = peakHour.City;
            existingPeakHour.PeakSunlightHour = peakHour.PeakSunlightHour;

            await dbContext.SaveChangesAsync();
            return existingPeakHour;
        }
    
    }
}
