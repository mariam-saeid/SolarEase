using SolarEase.Data;
using SolarEase.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace SolarEase.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly SolarEaseAuthDbContext dbContext;
        public AdminRepository(SolarEaseAuthDbContext dbontext)
        {
            this.dbContext = dbontext;
        }

        public async Task<List<Admin>> GetAllAsync()
        {
            return await dbContext.Admins.Include(admin => admin.Profile).Include(admin => admin.Account).ToListAsync();
        }

        public async Task<Admin?> GetByIdAsync(int id)
        {
            return await dbContext.Admins.Include(admin => admin.Profile).Include(admin => admin.Account).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Admin?> GetByEmailAsync(string Email)
        {
            return await dbContext.Admins.Include(admin => admin.Profile).Include(admin => admin.Account).FirstOrDefaultAsync(r => r.Account.Email == Email);
        }

        public async Task<Admin> CreateAsync(Admin adminModel)
        {
            await dbContext.Admins.AddAsync(adminModel);
            await dbContext.SaveChangesAsync();
            return adminModel;
        }

        public async Task<Admin?> UpdateAsync(int id, Admin adminModel)
        {
            var existingAdmin = await dbContext.Admins.Include(admin => admin.Profile).Include(admin => admin.Account).FirstOrDefaultAsync(x => x.Id == id);
            if (existingAdmin == null)
            {
                return null;
            }
            existingAdmin.Name = adminModel.Name;
            existingAdmin.Location = adminModel.Location;
            existingAdmin.City = adminModel.City;
            existingAdmin.PhoneNumber = adminModel.PhoneNumber;

            await dbContext.SaveChangesAsync();
            return existingAdmin;
        }

        public async Task<Admin?> DeleteAsync(int id)
        {
            var adminModel = await dbContext.Admins.Include(admin => admin.Profile).Include(admin => admin.Account).FirstOrDefaultAsync(x => x.Id == id);
            if (adminModel == null)
            {
                return null;
            }
            dbContext.Admins.Remove(adminModel);
            await dbContext.SaveChangesAsync();
            return adminModel;
        }

    }
}
