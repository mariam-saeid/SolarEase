using SolarEase.Models.Domain;
using SolarEase.Models.DTO.FavoriteDto;
using SolarEase.Models.Domain;
using static System.Net.Mime.MediaTypeNames;

namespace SolarEase.Repositories
{
    public interface IFavoriteProductRepository
    {
        Task<FavoriteProduct?> GetByIdAsync(int personId, int productId);
        Task<List<FavoriteProduct>> GetAllAsync();
        Task<List<SolarProduct>?> GetAllByPersonAsync(int personId, String? query);
        Task<List<Person>?> GetAllByProductAsync(int productId);
        Task<List<FavoriteProduct>?> DeleteAllByPersonAsync(int personId);
        Task<List<FavoriteProduct>?> DeleteAllByProductAsync(int productId);
        Task<FavoriteProduct> CreateAsync(FavoriteProduct favoriteProduct);
        Task<FavoriteProduct?> DeleteAsync(int personId, int productId);
        
    }
}
