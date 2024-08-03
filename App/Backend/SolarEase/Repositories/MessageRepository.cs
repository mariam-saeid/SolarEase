using Microsoft.EntityFrameworkCore;
using SolarEase.Data;
using SolarEase.Models.Domain;

namespace SolarEase.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly SolarEaseAuthDbContext dbContext;
        public MessageRepository(SolarEaseAuthDbContext dbontext)
        {
            this.dbContext = dbontext;
        }

        public async Task<List<Message>> GetAllAsync()
        {
            var messages = await dbContext.Messages.ToListAsync();

            return messages;

        }

        public async Task<List<Message>?> GetAllByPersonAsync(int personId)
        {
            if (!await dbContext.Persons.AnyAsync(p => p.Id == personId))
                return null;

            var messages = await dbContext.Messages
                .Where(c => c.PersonId == personId).OrderByDescending(c => c.SentDate)
                .ToListAsync();

            return messages;

        }

        public async Task<Message?> GetByIdAsync(int id)
        {
            return await dbContext.Messages.FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Message> CreateAsync(Message message)
        {
            await dbContext.Messages.AddAsync(message);
            await dbContext.SaveChangesAsync();
            return message;
        }

        public async Task<Message> UpdateAsync(int id, Message message)
        {
            var existingMessage = await dbContext.Messages.FirstOrDefaultAsync(x => x.Id == id);
            if (existingMessage == null)
            {
                return null;
            }
            existingMessage.Title = message.Title;
            existingMessage.Body = message.Body;

            await dbContext.SaveChangesAsync();
            return existingMessage;
        }

        public async Task<Message?> DeleteAsync(int id)
        {
            var message = await dbContext.Messages.FirstOrDefaultAsync(x => x.Id == id);
            if (message == null)
            {
                return null;
            }
            dbContext.Messages.Remove(message);
            await dbContext.SaveChangesAsync();
            return message;
        }

        public async Task<List<Message>?> DeleteAllByPersonAsync(int personId)
        {
            if (!await dbContext.Persons.AnyAsync(p => p.Id == personId))
                return null;

            var messages = await dbContext.Messages.Where(c => c.PersonId == personId).ToListAsync();

            dbContext.Messages.RemoveRange(messages);
            await dbContext.SaveChangesAsync();

            return messages;
        }
    }
}
