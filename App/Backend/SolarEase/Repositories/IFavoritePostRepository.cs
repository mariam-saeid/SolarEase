using SolarEase.Models.Domain;
using static System.Net.Mime.MediaTypeNames;

namespace SolarEase.Repositories
{
    public interface IFavoritePostRepository
    {
        Task<FavoritePost> CreateAsync(FavoritePost favoritePostModel);
        Task<FavoritePost?> DeleteAsync(int PersonId, int PostId);
        Task<FavoritePost?> GetByIdAsync(int PersonId, int PostId);
        Task<List<FavoritePost>> GetAllAsync();
        Task<List<Post>?> GetAllByPersonAsync(int PersonId, String? query);
        Task<List<Person>?> GetAllByPostAsync(int PostId);
        Task<List<FavoritePost>?> DeleteAllByPersonAsync(int personId);
        Task<List<FavoritePost>?> DeleteAllByPostAsync(int postId);
    }
}
