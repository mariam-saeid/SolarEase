using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SolarEase.Data;
using SolarEase.Models.Domain;
using System.Globalization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace SolarEase.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly SolarEaseAuthDbContext dbContext;
        public PostRepository(SolarEaseAuthDbContext dbontext)
        {
            this.dbContext = dbontext;
        }

        public async Task<List<Post>> GetAllQueryAsync(string? sortBy = null, String? categoryQuery = null, String? cityQuery = null)
        {
            var posts = dbContext.Posts
                .Include(post => post.Person)
                .ThenInclude(person => person.Profile) // Include Profile object from Person entity
                .Include(post => post.SolarProduct)
                .ThenInclude(solarProduct => solarProduct.ProductCategory) // Include ProductCategory object from SolarProduct entity
                .AsQueryable();

            if (!String.IsNullOrEmpty(categoryQuery))
            {
                posts = posts.Where(p => p.SolarProduct.ProductCategory.Name.ToLower().Contains(categoryQuery.ToLower()));
            }
            if (!String.IsNullOrEmpty(cityQuery))
            {
                posts = posts.Where(p => p.City.ToLower().Contains(cityQuery.ToLower()));
            }
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "a":
                        posts = posts.OrderBy(p => p.CreatedOn);
                        break;
                    case "d":
                        posts = posts.OrderByDescending(p => p.CreatedOn);
                        break;
                    default:
                        posts = posts.OrderByDescending(p => p.CreatedOn);
                        break;
                }
            }
            else
            {
                posts = posts.OrderByDescending(p => p.CreatedOn);
            }
            return await posts.ToListAsync();
        }

        public async Task<List<Post>> GetAllActiveQueryAsync(int personId,string? sortBy = null, String ? categoryQuery = null, String? cityQuery = null)
        {
            var posts = dbContext.Posts.Where(p => p.Active && p.PersonId != personId )
                .Include(post => post.Person)
                .ThenInclude(person => person.Profile) // Include Profile object from Person entity
                .Include(post => post.SolarProduct)
                .ThenInclude(solarProduct => solarProduct.ProductCategory) // Include ProductCategory object from SolarProduct entity
                .AsQueryable();

            if (!String.IsNullOrEmpty(categoryQuery))
            {
                posts = posts.Where(p => p.SolarProduct.ProductCategory.Name.ToLower().Contains(categoryQuery.ToLower()));
            }
            if (!String.IsNullOrEmpty(cityQuery))
            {
                posts = posts.Where(p => p.City.ToLower().Contains(cityQuery.ToLower()));
            }
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "a":
                        posts = posts.OrderBy(p => p.CreatedOn);
                        break;
                    case "d":
                        posts = posts.OrderByDescending(p => p.CreatedOn);
                        break;
                    default:
                        posts = posts.OrderByDescending(p => p.CreatedOn);
                        break;
                }
            }
            else
            {
                posts = posts.OrderByDescending(p => p.CreatedOn);
            }
            return await posts.ToListAsync();
        }

        public async Task<List<Post>> GetAllInActiveQueryAsync(string? sortBy = null, String? categoryQuery = null, String? cityQuery = null)
        {
            var posts = dbContext.Posts.Where(p => !p.Active)
                .Include(post => post.Person)
                .ThenInclude(person => person.Profile) // Include Profile object from Person entity
                .Include(post => post.SolarProduct)
                .ThenInclude(solarProduct => solarProduct.ProductCategory) // Include ProductCategory object from SolarProduct entity
                .AsQueryable();

            if (!String.IsNullOrEmpty(categoryQuery))
            {
                posts = posts.Where(p => p.SolarProduct.ProductCategory.Name.ToLower().Contains(categoryQuery.ToLower()));
            }
            if (!String.IsNullOrEmpty(cityQuery))
            {
                posts = posts.Where(p => p.City.ToLower().Contains(cityQuery.ToLower()));
            }
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "a":
                        posts = posts.OrderBy(p => p.CreatedOn);
                        break;
                    case "d":
                        posts = posts.OrderByDescending(p => p.CreatedOn);
                        break;
                    default:
                        posts = posts.OrderByDescending(p => p.CreatedOn);
                        break;
                }
            }
            else
            {
                posts = posts.OrderByDescending(p => p.CreatedOn);
            }
            return await posts.ToListAsync();
        }

        public async Task<List<Post>> GetAllPersonActiveQueryAsync(int personId,string? sortBy = null, String? categoryQuery = null, String? cityQuery = null)
        {
            var posts = dbContext.Posts.Where(p => p.PersonId == personId && p.Active)
                .Include(post => post.Person)
                .ThenInclude(person => person.Profile) // Include Profile object from Person entity
                .Include(post => post.SolarProduct)
                .ThenInclude(solarProduct => solarProduct.ProductCategory) // Include ProductCategory object from SolarProduct entity
                .AsQueryable();


            if (!String.IsNullOrEmpty(categoryQuery))
            {
                posts = posts.Where(p => p.SolarProduct.ProductCategory.Name.ToLower().Contains(categoryQuery.ToLower()));
            }
            if (!String.IsNullOrEmpty(cityQuery))
            {
                posts = posts.Where(p => p.City.ToLower().Contains(cityQuery.ToLower()));
            }
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "a":
                        posts = posts.OrderBy(p => p.CreatedOn);
                        break;
                    case "d":
                        posts = posts.OrderByDescending(p => p.CreatedOn);
                        break;
                    default:
                        posts = posts.OrderByDescending(p => p.CreatedOn);
                        break;
                }
            }
            else
            {
                posts = posts.OrderByDescending(p => p.CreatedOn);
            }
            return await posts.ToListAsync();
        }

        public async Task<List<Post>> GetAllPersonInActiveQueryAsync(int personId, string? sortBy = null, String? categoryQuery = null, String? cityQuery = null)
        {
            var posts = dbContext.Posts.Where(p => p.PersonId == personId && !p.Active)
                .Include(post => post.Person)
                .ThenInclude(person => person.Profile) // Include Profile object from Person entity
                .Include(post => post.SolarProduct)
                .ThenInclude(solarProduct => solarProduct.ProductCategory) // Include ProductCategory object from SolarProduct entity
                .AsQueryable();

            if (!String.IsNullOrEmpty(categoryQuery))
            {
                posts = posts.Where(p => p.SolarProduct.ProductCategory.Name.ToLower().Contains(categoryQuery.ToLower()));
            }
            if (!String.IsNullOrEmpty(cityQuery))
            {
                posts = posts.Where(p => p.City.ToLower().Contains(cityQuery.ToLower()));
            }
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "a":
                        posts = posts.OrderBy(p => p.CreatedOn);
                        break;
                    case "d":
                        posts = posts.OrderByDescending(p => p.CreatedOn);
                        break;
                    default:
                        posts = posts.OrderByDescending(p => p.CreatedOn);
                        break;
                }
            }
            else
            {
                posts = posts.OrderByDescending(p => p.CreatedOn);
            }
            return await posts.ToListAsync();
        }

        public async Task<List<Post>> GetAllPersonAsync(int personId, string? sortBy = null, String? categoryQuery = null, String? cityQuery = null)
        {
            var posts = dbContext.Posts.Where(p => p.PersonId == personId)
                .Include(post => post.Person)
                .ThenInclude(person => person.Profile) // Include Profile object from Person entity
                .Include(post => post.SolarProduct)
                .ThenInclude(solarProduct => solarProduct.ProductCategory) // Include ProductCategory object from SolarProduct entity
                .AsQueryable();

            if (!String.IsNullOrEmpty(categoryQuery))
            {
                posts = posts.Where(p => p.SolarProduct.ProductCategory.Name.ToLower().Contains(categoryQuery.ToLower()));
            }
            if (!String.IsNullOrEmpty(cityQuery))
            {
                posts = posts.Where(p => p.City.ToLower().Contains(cityQuery.ToLower()));
            }
            if (!string.IsNullOrEmpty(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "a":
                        posts = posts.OrderBy(p => p.CreatedOn);
                        break;
                    case "d":
                        posts = posts.OrderByDescending(p => p.CreatedOn);
                        break;
                    default:
                        posts = posts.OrderByDescending(p => p.CreatedOn);
                        break;
                }
            }
            else
            {
                posts = posts.OrderByDescending(p => p.CreatedOn);
            }
            return await posts.ToListAsync();
        }

        public async Task<Post?> GetByIdInfoAsync(int id)
        {
            return await dbContext.Posts
                 .Include(post => post.Person)
                .ThenInclude(person => person.Profile) // Include Profile object from Person entity
                .Include(post => post.SolarProduct)
                .ThenInclude(solarProduct => solarProduct.ProductCategory) // Include ProductCategory object from SolarProduct entity
                .FirstOrDefaultAsync(p => p.Id == id);

        }

        public async Task<Post?> GetByIdActiveInfoAsync(int id)
        {
            return await dbContext.Posts
                .Include(post => post.Person)
                .ThenInclude(person => person.Profile) // Include Profile object from Person entity
                .Include(post => post.SolarProduct)
                .ThenInclude(solarProduct => solarProduct.ProductCategory) // Include ProductCategory object from SolarProduct entity
                .FirstOrDefaultAsync(p => p.Id == id && p.Active);

        }
        
        public async Task<Post?> GetByIdInActiveInfoAsync(int id)
        {
            return await dbContext.Posts
                .Include(post => post.Person)
                .ThenInclude(person => person.Profile) // Include Profile object from Person entity
                .Include(post => post.SolarProduct)
                .ThenInclude(solarProduct => solarProduct.ProductCategory) // Include ProductCategory object from SolarProduct entity
                .FirstOrDefaultAsync(p => p.Id == id && !p.Active);

        }

        public async Task<Post> CreateAsync(Post postModel)
        {
            await dbContext.Posts.AddAsync(postModel);
            await dbContext.SaveChangesAsync();
            return postModel;
        }

        public async Task<Post?> UpdateAsync(int id, Post postModel)
        {
            var existingPosts = await dbContext.Posts
                .Include(post => post.Person)
                .ThenInclude(person => person.Profile)
                .Include(post => post.SolarProduct)
                .ThenInclude(solarProduct => solarProduct.ProductCategory)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existingPosts == null)
            {
                return null;
            }
            existingPosts.IsUsed = postModel.IsUsed;
            existingPosts.Description = postModel.Description;
            existingPosts.Location = postModel.Location;
            existingPosts.City= postModel.City;
            existingPosts.Active = postModel.Active;
            existingPosts.CreatedOn = postModel.CreatedOn;

            await dbContext.SaveChangesAsync();
            return existingPosts;
        }

        public async Task<Post?> DeleteAsync(int id)
        {
            var postModel = await dbContext.Posts.Include(post => post.Person)
                .ThenInclude(person => person.Profile) // Include Profile object from Person entity
                .Include(post => post.SolarProduct)
                .ThenInclude(solarProduct => solarProduct.ProductCategory)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (postModel == null)
            {
                return null;
            }
            dbContext.Posts.Remove(postModel);
            await dbContext.SaveChangesAsync();
            return postModel;
        }

        public async Task<Post?> ApprovePostAsync(int id)
        {
            var existingPost = await dbContext.Posts
                .Include(post => post.Person)
                .ThenInclude(person => person.Profile)
                .Include(post => post.Person)
                .ThenInclude(person => person.Account)
                .Include(post => post.SolarProduct)
                .ThenInclude(solarProduct => solarProduct.ProductCategory)
                .FirstOrDefaultAsync(p => p.Id == id && !p.Active);
            
            if (existingPost == null) { return null; }
            existingPost.Active = true;
            await dbContext.SaveChangesAsync();
            return existingPost;
        }

        public async Task<Post?> CheckRejectPostAsync(int id)
        {
            var existingPost = await dbContext.Posts.Include(post => post.Person)
                .ThenInclude(person => person.Profile)
                .Include(post => post.Person)
                .ThenInclude(person => person.Account)
                .Include(post => post.SolarProduct)
                .ThenInclude(solarProduct => solarProduct.ProductCategory)
                .FirstOrDefaultAsync(p => p.Id == id && !p.Active);
           
            if (existingPost == null) { return null; }
            return existingPost;
        }

    }
}
