using SolarEase.Data;
using SolarEase.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace SolarEase.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly SolarEaseAuthDbContext dbContext;
        public PersonRepository(SolarEaseAuthDbContext dbontext)
        {
            this.dbContext = dbontext;
        }

        public async Task<List<Person>> GetAllAsync()
        {
            return await dbContext.Persons.Include(person => person.Profile).Include(person => person.Account).ToListAsync();
        }

        public async Task<Person?> GetByIdAsync(int id)
        {
            return await dbContext.Persons.Include(person => person.Profile).Include(person => person.Account).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Person?> GetByEmailAsync(string Email)
        {
            return await dbContext.Persons.Include(person => person.Profile).Include(person => person.Account).FirstOrDefaultAsync(r => r.Account.Email == Email);
        }

        public async Task<Person> CreateAsync(Person personModel)
        {
            await dbContext.Persons.AddAsync(personModel);
            await dbContext.SaveChangesAsync();
            return personModel;
        }

        public async Task<Person?> DeleteAsync(int id)
        {
            var personModel = await dbContext.Persons.Include(person => person.Profile).Include(person => person.Account).FirstOrDefaultAsync(x => x.Id == id);
            if (personModel == null)
            {
                return null;
            }
            dbContext.Persons.Remove(personModel);
            await dbContext.SaveChangesAsync();
            return personModel;
        }

        public async Task<Person?> UpdateAsync(int id, Person personModel)
        {
            var existingPerson = await dbContext.Persons.Include(person => person.Profile).Include(person => person.Account).FirstOrDefaultAsync(x => x.Id == id);
            if (existingPerson == null)
            {
                return null;
            }
            existingPerson.Name = personModel.Name;
            existingPerson.PhoneNumber = personModel.PhoneNumber;
            existingPerson.Location = personModel.Location;
            existingPerson.City = personModel.City;

            await dbContext.SaveChangesAsync();
            return existingPerson;
        }
        
        public async Task<Person?> UpdateAllAsync(int id, Person personModel)
        {
            var existingPerson = await dbContext.Persons.Include(person => person.Profile).Include(person => person.Account).FirstOrDefaultAsync(x => x.Id == id);
            if (existingPerson == null)
            {
                return null;
            }
            existingPerson.Name = personModel.Name;
            existingPerson.PhoneNumber = personModel.PhoneNumber;
            existingPerson.Location = personModel.Location;
            existingPerson.City = personModel.City;
            existingPerson.SystemSize = personModel.SystemSize;

            await dbContext.SaveChangesAsync();
            return existingPerson;
        }
    
    }
}
