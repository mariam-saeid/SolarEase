using Microsoft.EntityFrameworkCore;
using SolarEase.Data;
using SolarEase.Models.Domain;

namespace SolarEase.Repositories
{
    public class ChatbotMessageRepository : IChatbotMessageRepository
    {
        private readonly SolarEaseAuthDbContext dbContext;
        public ChatbotMessageRepository(SolarEaseAuthDbContext dbontext)
        {
            this.dbContext = dbontext;
        }

        public async Task<List<ChatbotMessage>> GetAllAsync()
        {
            var chatbotMessages = await dbContext.ChatbotMessages
            .Include(c => c.Person).ThenInclude(person => person.Profile).ToListAsync();

            return chatbotMessages;

        }

        //.OrderBy(c => c.Date)

        public async Task<List<ChatbotMessage>?> GetAllByPersonAsync(int personId)
        {
            if (!await dbContext.Persons.AnyAsync(p => p.Id == personId))
                return null;

            var chatbotMessages = await dbContext.ChatbotMessages
            .Include(c => c.Person).ThenInclude(person => person.Profile)
                .Where(c => c.PersonId == personId).OrderBy(c => c.Date)  // Order by Date only (ignoring time)
                .ToListAsync();

            return chatbotMessages;

        }

        public async Task<ChatbotMessage?> GetByIdAsync(int id)
        {
            return await dbContext.ChatbotMessages.Include(c => c.Person).ThenInclude(person => person.Profile).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<ChatbotMessage> CreateAsync(ChatbotMessage chatbotMessage)
        {
            await dbContext.ChatbotMessages.AddAsync(chatbotMessage);
            await dbContext.SaveChangesAsync();
            return chatbotMessage;
        }

        public async Task<ChatbotMessage> UpdateAsync(int id, ChatbotMessage chatbotMessage)
        {
            var existingChatbotMessage = await dbContext.ChatbotMessages.Include(c => c.Person).ThenInclude(person => person.Profile).FirstOrDefaultAsync(x => x.Id == id);
            if (existingChatbotMessage == null)
            {
                return null;
            }
            existingChatbotMessage.Answer = chatbotMessage.Answer;
            existingChatbotMessage.Question = chatbotMessage.Question;

            await dbContext.SaveChangesAsync();
            return existingChatbotMessage;
        }

        public async Task<ChatbotMessage?> DeleteAsync(int id)
        {
            var chatbotMessage = await dbContext.ChatbotMessages.Include(c => c.Person).ThenInclude(person => person.Profile).FirstOrDefaultAsync(x => x.Id == id);
            if (chatbotMessage == null)
            {
                return null;
            }
            dbContext.ChatbotMessages.Remove(chatbotMessage);
            await dbContext.SaveChangesAsync();
            return chatbotMessage;
        }

        public async Task<List<ChatbotMessage>?> DeleteAllByPersonAsync(int personId)
        {
            if (!await dbContext.Persons.AnyAsync(p => p.Id == personId))
                return null;

            var chatbotMessages = await dbContext.ChatbotMessages.Include(c => c.Person).ThenInclude(person => person.Profile).Where(c => c.PersonId == personId).ToListAsync();

            dbContext.ChatbotMessages.RemoveRange(chatbotMessages);
            await dbContext.SaveChangesAsync();

            return chatbotMessages;
        }
    }
}
