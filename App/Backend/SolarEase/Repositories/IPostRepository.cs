using SolarEase.Models.Domain;

namespace SolarEase.Repositories
{
    public interface IPostRepository
    {
        Task<List<Post>> GetAllQueryAsync(string? sortBy = null, String ? categoryQuery = null, String? cityQuery = null);
        Task<List<Post>> GetAllActiveQueryAsync(int personId,string? sortBy = null, String ? categoryQuery = null, String? cityQuery = null);
        Task<List<Post>> GetAllInActiveQueryAsync(string? sortBy = null, String ? categoryQuery = null, String? cityQuery = null);
        Task<List<Post>> GetAllPersonActiveQueryAsync(int personId, string? sortBy = null, String? ucategoryQueryery = null,String? cityQuery = null);
        Task<List<Post>> GetAllPersonInActiveQueryAsync(int personId, string? sortBy = null, String? categoryQuery = null, String? cityQuery = null);
        Task<List<Post>> GetAllPersonAsync(int personId, string? sortBy = null, String? categoryQuery = null, String? cityQuery = null);
        Task<Post?> GetByIdInfoAsync(int id);
        Task<Post?> GetByIdActiveInfoAsync(int id);
        Task<Post?> GetByIdInActiveInfoAsync(int id);
        Task<Post> CreateAsync(Post postModel);
        Task<Post?> DeleteAsync(int id);
        Task<Post?> UpdateAsync(int id, Post postModel);
        Task<Post?> ApprovePostAsync(int id);
        Task<Post?> CheckRejectPostAsync(int id);
    }
}
