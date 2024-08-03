using SolarEase.Data;
using SolarEase.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace SolarEase.Repositories
{
    public class SolarInstallerRepository : ISolarInstallerRepository
    {
        private readonly SolarEaseAuthDbContext dbContext;
        public SolarInstallerRepository(SolarEaseAuthDbContext dbontext)
        {
            this.dbContext = dbontext;
        }

        public async Task<List<SolarInstaller>> GetAllQueryAsync(String? nameQuery = null, String? cityQuery = null)
        {
            var installers = dbContext.SolarInstallers.AsQueryable();
            if (!String.IsNullOrEmpty(nameQuery))
            {
                installers = installers.Where(i => i.Name.ToLower().Contains(nameQuery.ToLower()));
            }
            if (!String.IsNullOrEmpty(cityQuery))
            {
                installers = installers.Where(i => i.City.ToLower().Contains(cityQuery.ToLower()));
            }
            return await installers.ToListAsync();
        }

        public async Task<SolarInstaller?> GetByIdAsync(int id)
        {
            return await dbContext.SolarInstallers.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<SolarInstaller> CreateAsync(SolarInstaller solarInstallerModel)
        {
            await dbContext.SolarInstallers.AddAsync(solarInstallerModel);
            await dbContext.SaveChangesAsync();
            return solarInstallerModel;
        }

        public async Task<SolarInstaller?> DeleteAsync(int id)
        {
            var solarInstallerModel = await dbContext.SolarInstallers.FirstOrDefaultAsync(x => x.Id == id);
            if (solarInstallerModel == null)
            {
                return null;
            }
            dbContext.SolarInstallers.Remove(solarInstallerModel);
            await dbContext.SaveChangesAsync();
            return solarInstallerModel;
        }

        public async Task<SolarInstaller?> UpdateAsync(int id, SolarInstaller solarInstallerModel)
        {
            var existingSolarInstaller = await dbContext.SolarInstallers.FirstOrDefaultAsync(x => x.Id == id);
            if (existingSolarInstaller == null)
            {
                return null;
            }
            existingSolarInstaller.Name = solarInstallerModel.Name;
            existingSolarInstaller.Email = solarInstallerModel.Email;
            existingSolarInstaller.PhoneNumber = solarInstallerModel.PhoneNumber;
            existingSolarInstaller.Address = solarInstallerModel.Address;
            existingSolarInstaller.City = solarInstallerModel.City;

            await dbContext.SaveChangesAsync();
            return existingSolarInstaller;
        }
    
    }
}
