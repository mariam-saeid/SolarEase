using SolarEase.Data;
using SolarEase.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace SolarEase.Repositories
{
    public class SolarProductRepository : ISolarProductRepository
    {
        private readonly SolarEaseAuthDbContext dbContext;
        public SolarProductRepository(SolarEaseAuthDbContext dbontext)
        {
            this.dbContext = dbontext;
        }

        public async Task<List<SolarProduct>> GetAllAsync(String? query)
        {
            var solarProducts = dbContext.SolarProducts.Include(p => p.ProductCategory).Where(p => p.IsProductPost == false).AsQueryable();

            if (!String.IsNullOrEmpty(query))
            {
                solarProducts = solarProducts.Where(j => j.ProductCategory.Name.ToLower().Contains(query.ToLower()));
                solarProducts = solarProducts.OrderBy(p => p.Brand).ThenBy(p => p.Capacity);
            }
            else
            {
                solarProducts = solarProducts.OrderBy(p => p.ProductCategory.Name).ThenBy(p => p.Brand).ThenBy(p => p.Capacity);
            }
            return await solarProducts.ToListAsync();

        }

        public async Task<List<SolarProduct>> GetByCategoryNameAsync(string categoryName)
        {
            var solarProducts = dbContext.SolarProducts
                .Include(p => p.ProductCategory)
                .Where(p => p.ProductCategory.Name == categoryName).Where(p => p.IsProductPost == false);

            solarProducts = solarProducts.OrderBy(p => p.Brand).ThenBy(p => p.Capacity);
            return await solarProducts.ToListAsync();
        }

        public async Task<List<SolarProduct>> GetAllCapacitySortedAsync(string categoryName, double minCapacity, double maxCapacity)
        {
            var solarProducts = dbContext.SolarProducts
                .Include(p => p.ProductCategory)
                .Where(p => p.ProductCategory.Name == categoryName
                            && p.IsProductPost == false
                            && p.Capacity >= minCapacity
                            && p.Capacity <= maxCapacity);

            solarProducts = solarProducts.OrderBy(p => p.Capacity);

            return await solarProducts.ToListAsync();
        }

        public async Task<double> GetPriceByCapacityAsync(double capacity, string categoryName, string? brandName)
        {
            var solarProducts = dbContext.SolarProducts.Include(p => p.ProductCategory)
                .Where(p => p.ProductCategory.Name == categoryName && p.IsProductPost == false && p.Capacity == capacity)
                .OrderBy(p => p.Capacity)
                .AsQueryable();

            if (!string.IsNullOrEmpty(brandName))
            {
                solarProducts = solarProducts.Where(j => j.Brand.ToLower().Contains(brandName.ToLower()));
            }

            double avgSolarProductsPrices = await solarProducts.Select(p => p.Price).MaxAsync();

            return avgSolarProductsPrices;
        }

        public async Task<double> GetNextHigherCapacityAsync(string categoryName, double capacity, string? brandName)
        {
            var solarProducts = dbContext.SolarProducts.Include(p => p.ProductCategory)
                .Where(p => p.ProductCategory.Name == categoryName && p.IsProductPost == false && p.Capacity >= capacity)
                .OrderBy(p => p.Capacity)
                .AsQueryable();

            if (!string.IsNullOrEmpty(brandName)) 
            {
                solarProducts = solarProducts.Where(j => j.Brand.ToLower().Contains(brandName.ToLower()));
            }

            var nextCapacity = await solarProducts
                .Select(p => (double?)p.Capacity) // Cast to nullable double to handle null case
                .FirstOrDefaultAsync();

            if (nextCapacity == null)
            {
                solarProducts = dbContext.SolarProducts.Include(p => p.ProductCategory)
                   .Where(p => p.ProductCategory.Name == categoryName && p.IsProductPost == false)
                   .AsQueryable();

                if (!string.IsNullOrEmpty(brandName))
                {
                    solarProducts = solarProducts.Where(j => j.Brand.ToLower().Contains(brandName.ToLower()));
                }

                var maxCapacity = await solarProducts.MaxAsync(p => (double?)p.Capacity);

                return (double)maxCapacity; 
            }

            return (double)nextCapacity; 
        }

        public async Task<SolarProduct?> GetByIdAsync(int id)
        {
            return await dbContext.SolarProducts.Include(p => p.ProductCategory).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<SolarProduct> CreateAsync(SolarProduct solarProductModel)
        {
            await dbContext.SolarProducts.AddAsync(solarProductModel);
            await dbContext.SaveChangesAsync();
            return solarProductModel;
        }
       
        public async Task<SolarProduct?> DeleteAsync(int id)
        {
            var solarProductModel = await dbContext.SolarProducts.Include(p => p.ProductCategory).FirstOrDefaultAsync(x => x.Id == id);
            if (solarProductModel == null)
            {
                return null;
            }
            dbContext.SolarProducts.Remove(solarProductModel);
            await dbContext.SaveChangesAsync();
            return solarProductModel;
        }

        public async Task<SolarProduct?> UpdateAsync(int id, SolarProduct solarProductModel)
        {
            var existingProduct = await dbContext.SolarProducts.Include(p => p.ProductCategory).FirstOrDefaultAsync(x => x.Id == id);
            if (existingProduct == null)
            {
                return null;
            }
           
            existingProduct.Price= solarProductModel.Price;
            existingProduct.Brand = solarProductModel.Brand;
            existingProduct.Capacity = solarProductModel.Capacity;
            existingProduct.ImageUrl = solarProductModel.ImageUrl;
            existingProduct.ProductCategoryId=solarProductModel.ProductCategoryId;
            existingProduct.MeasuringUnit = solarProductModel.MeasuringUnit;
            existingProduct.Volt = solarProductModel.Volt;

            await dbContext.SaveChangesAsync();
            return existingProduct;
        }

    }
}
