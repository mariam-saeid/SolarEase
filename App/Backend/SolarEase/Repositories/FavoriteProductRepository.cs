using SolarEase.Models.Domain;
using SolarEase.Models.DTO.FavoriteDto;
using Microsoft.EntityFrameworkCore;
using SolarEase.Data;
using SolarEase.Models.Domain;
using Microsoft.Extensions.Hosting;

namespace SolarEase.Repositories
{
    public class FavoriteProductRepository:IFavoriteProductRepository
    {

        private readonly SolarEaseAuthDbContext dbContext;
        public FavoriteProductRepository(SolarEaseAuthDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //Favorite favoriteModel
        public async Task<List<FavoriteProduct>> GetAllAsync()
        {
            return await dbContext.FavoriteProducts.Include(f => f.Person).ThenInclude(p=>p.Profile).
                Include(f => f.Product).ThenInclude(p=>p.ProductCategory).ToListAsync();
        }
        
        public async Task<FavoriteProduct?> GetByIdAsync(int personId, int productId)
        {
            return await dbContext.FavoriteProducts.Include(f => f.Person).ThenInclude(p => p.Profile)
                .Include(f => f.Product).ThenInclude(p => p.ProductCategory)
                .FirstOrDefaultAsync(f => f.PersonId == personId && f.ProductId == productId);
        }

        public async Task<List<SolarProduct>?> GetAllByPersonAsync(int personId, String? query)
        {
            if (!await dbContext.Persons.AnyAsync(p => p.Id == personId))
                return null;

            var products = dbContext.FavoriteProducts.Include(p => p.Product)
                .ThenInclude(p => p.ProductCategory)
                .Where(f => f.PersonId == personId).OrderByDescending(f => f.AddedDate)
                .Select(f => f.Product).AsQueryable();

            if (!String.IsNullOrEmpty(query))
            {
                products = products.Where(j => j.ProductCategory.Name.ToLower().Contains(query.ToLower()));
            }

            return await products.ToListAsync();
        }

        public async Task<List<Person>?> GetAllByProductAsync(int productId)
        {
            if (!await dbContext.SolarProducts.AnyAsync(p => p.Id == productId))
                return null;

            var persons = await dbContext.FavoriteProducts.Include(f => f.Person)
                .ThenInclude(p => p.Profile).Include(f => f.Person).ThenInclude(p => p.Account)
                .Where(f => f.ProductId == productId).Select(f => f.Person).ToListAsync();

            return persons;
        }

        public async Task<List<FavoriteProduct>?> DeleteAllByPersonAsync(int personId)
        {
            if (!await dbContext.Persons.AnyAsync(p => p.Id == personId))
                return null;

            var favoriteProduct = await dbContext.FavoriteProducts.Where(f => f.PersonId == personId).ToListAsync();

            dbContext.FavoriteProducts.RemoveRange(favoriteProduct);
            await dbContext.SaveChangesAsync();

            return favoriteProduct;
        }

        public async Task<List<FavoriteProduct>?> DeleteAllByProductAsync(int productId)
        {
            if (!await dbContext.SolarProducts.AnyAsync(p => p.Id == productId))
                return null;

            var favoriteProduct = await dbContext.FavoriteProducts.Where(f => f.ProductId == productId).ToListAsync();

            dbContext.FavoriteProducts.RemoveRange(favoriteProduct);
            await dbContext.SaveChangesAsync();

            return favoriteProduct;
        }

        public async Task<FavoriteProduct> CreateAsync(FavoriteProduct favoriteProduct)
        {
            await dbContext.FavoriteProducts.AddAsync(favoriteProduct);
            await dbContext.SaveChangesAsync();
            return favoriteProduct;
        }

        public async Task<FavoriteProduct?> DeleteAsync(int personId, int productId)
        {
            var favoriteProductModel = await dbContext.FavoriteProducts.Include(f => f.Person).ThenInclude(p => p.Profile)
                .Include(f => f.Product).ThenInclude(p => p.ProductCategory)
                 .FirstOrDefaultAsync(f => f.PersonId == personId && f.ProductId == productId);
            if (favoriteProductModel == null)
            {
                return null;
            }
            dbContext.FavoriteProducts.Remove(favoriteProductModel);
            await dbContext.SaveChangesAsync();
            return favoriteProductModel;
        }

        
    }
}
