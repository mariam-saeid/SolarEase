using SolarEase.Models.Domain;

namespace SolarEase.Repositories
{
    public interface IMessageRepository
    {
        Task<List<Message>> GetAllAsync();
        Task<List<Message>?> GetAllByPersonAsync(int personId);
        Task<Message?> GetByIdAsync(int id);
        Task<Message> CreateAsync(Message message);
        Task<Message> UpdateAsync(int id, Message message);
        Task<Message?> DeleteAsync(int id);
        Task<List<Message>?> DeleteAllByPersonAsync(int personId);    }
}
