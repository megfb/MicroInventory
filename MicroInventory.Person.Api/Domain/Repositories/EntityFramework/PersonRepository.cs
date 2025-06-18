using MicroInventory.Person.Api.Domain.Entities;
using MicroInventory.Person.Api.Domain.Repositories.Abstractions;
using MicroInventory.Person.Api.Domain.Repositories.EntityFramework.DbContexts;
using MicroInventory.Shared.Common.Domain;
using Microsoft.EntityFrameworkCore;

namespace MicroInventory.Person.Api.Domain.Repositories.EntityFramework
{
    public class PersonRepository(PersonDbContext context, IUnitOfWork unitOfWork) : IPersonRepository
    {
        public async Task<Persons> CreateAsync(Persons persons)
        {
            await context.Persons.AddAsync(persons);
            await unitOfWork.SaveChangesAsync();
            return persons;
        }
        public async Task DeleteAsync(Persons persons)
        {
            context.Persons.Remove(persons);
            await unitOfWork.SaveChangesAsync();
        }
        public async Task<IEnumerable<Persons>> GetAllAsync()
        {
            return await context.Persons.OrderByDescending(x => x.FirstName).ToListAsync();
        }
        public async Task<Persons> GetByIdAsync(string id)
        {
            return await context.Persons
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new KeyNotFoundException($"Person with ID {id} not found.");
        }
        public async Task UpdateAsync(Persons persons)
        {
            context.Persons.Update(persons);
            await unitOfWork.SaveChangesAsync();
        }
    }

}
