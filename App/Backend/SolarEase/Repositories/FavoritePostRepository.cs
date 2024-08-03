using SolarEase.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using SolarEase.Data;
using static System.Net.Mime.MediaTypeNames;

namespace SolarEase.Repositories
{
    public class FavoritePostRepository : IFavoritePostRepository
    {
        private readonly SolarEaseAuthDbContext dbContext;
        public FavoritePostRepository(SolarEaseAuthDbContext dbontext)
        {
            this.dbContext = dbontext;
        }
        
        public async Task<FavoritePost> CreateAsync(FavoritePost favoritePostModel)
        {
            await dbContext.FavoritePosts.AddAsync(favoritePostModel);
            await dbContext.SaveChangesAsync();
            return favoritePostModel;
        }

        public async Task<FavoritePost?> DeleteAsync(int PersonId, int PostId)
        {
            var favoritePostModel = await dbContext.FavoritePosts
                .FirstOrDefaultAsync(f => f.PersonId == PersonId && f.PostId == PostId);

            if (favoritePostModel == null)
                return null;

            dbContext.FavoritePosts.Remove(favoritePostModel);
            await dbContext.SaveChangesAsync();
            return favoritePostModel;
        }

        public async Task<FavoritePost?> GetByIdAsync(int PersonId, int PostId)
        {
            var favoritePostModel = await dbContext.FavoritePosts
                .Include(f => f.Person).ThenInclude(person => person.Profile)
                .Include(f => f.Post).ThenInclude(post => post.SolarProduct)
                .ThenInclude(solarProduct => solarProduct.ProductCategory)
                .FirstOrDefaultAsync(f => f.PersonId == PersonId && f.PostId == PostId);

            if (favoritePostModel == null)
                return null;
            return favoritePostModel;
        }

        public async Task<List<FavoritePost>> GetAllAsync()
        {
            var favoritePostModels = await dbContext.FavoritePosts
                .Include(f => f.Person).ThenInclude(person => person.Profile)
                .Include(f => f.Post).ThenInclude(post => post.SolarProduct)
                .ThenInclude(solarProduct => solarProduct.ProductCategory) 
                .ToListAsync();


            return favoritePostModels;

        }

        public async Task<List<Post>?> GetAllByPersonAsync(int PersonId, String? query)
        {
            if (!await dbContext.Persons.AnyAsync(p => p.Id == PersonId))
                return null;

            var posts = dbContext.FavoritePosts
                .Include(f => f.Post)
                .ThenInclude(post => post.Person)
                .ThenInclude(person => person.Profile)
                .Include(f => f.Post)
                .ThenInclude(post => post.SolarProduct)
                .ThenInclude(solarProduct => solarProduct.ProductCategory)
                .Where(f => f.PersonId == PersonId).OrderByDescending(f => f.AddedDate)
                .Select(f => f.Post)
                .AsQueryable();

            if (!String.IsNullOrEmpty(query))
            {
                posts = posts.Where(p => p.SolarProduct.ProductCategory.Name.ToLower().Contains(query.ToLower()));
            }


            return await posts.ToListAsync();

        }

        public async Task<List<Person>?> GetAllByPostAsync(int PostId)
        { 
            if (!await dbContext.Posts.AnyAsync(p => p.Id == PostId))
                return null;

            var persons = await dbContext.FavoritePosts
                .Include(f => f.Person)
                .ThenInclude(person => person.Profile)
                .Include(f => f.Person)
                .ThenInclude(person => person.Account)
                .Where(f => f.PostId == PostId)
                .Select(f => f.Person)
                .ToListAsync();


            return persons;

        }

        public async Task<List<FavoritePost>?> DeleteAllByPersonAsync(int personId)
        {
            if (!await dbContext.Persons.AnyAsync(p => p.Id == personId))
                return null;

            var favoritePosts = await dbContext.FavoritePosts.Where(f => f.PersonId == personId).ToListAsync();

            dbContext.FavoritePosts.RemoveRange(favoritePosts);
            await dbContext.SaveChangesAsync();

            return favoritePosts;
        }

        public async Task<List<FavoritePost>?> DeleteAllByPostAsync(int postId)
        {
            if (!await dbContext.Posts.AnyAsync(p => p.Id == postId))
                return null;

            var favoritePosts = await dbContext.FavoritePosts.Where(f => f.PostId == postId).ToListAsync();

            dbContext.FavoritePosts.RemoveRange(favoritePosts);
            await dbContext.SaveChangesAsync();

            return favoritePosts;
        }


    }
}

