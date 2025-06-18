using MicroInventory.Person.Api.Domain.Entities;

namespace MicroInventory.Person.Api.Domain.Repositories.Abstractions
{
    public interface IPersonRepository
    {
        Task<Persons> CreateAsync(Persons persons);
        Task UpdateAsync(Persons persons);
        Task DeleteAsync(Persons persons);
        Task<IEnumerable<Persons>> GetAllAsync();
        Task<Persons> GetByIdAsync(string id);
    }
}
