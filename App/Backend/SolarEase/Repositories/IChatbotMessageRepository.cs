using SolarEase.Models.Domain;

namespace SolarEase.Repositories
{
    public interface IChatbotMessageRepository
    {
        Task<ChatbotMessage> CreateAsync(ChatbotMessage chatbotMessage);
        Task<ChatbotMessage?> DeleteAsync(int chatbotMessageId);
        Task<ChatbotMessage?> GetByIdAsync(int chatbotMessageId);
        Task<List<ChatbotMessage>> GetAllAsync();
        Task<List<ChatbotMessage>?> GetAllByPersonAsync(int PersonId);
        Task<List<ChatbotMessage>?> DeleteAllByPersonAsync(int personId);
    }
}
