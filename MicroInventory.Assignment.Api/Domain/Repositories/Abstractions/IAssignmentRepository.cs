using MicroInventory.Assignment.Api.Domain.Entities;

namespace MicroInventory.Assignment.Api.Domain.Repositories.Abstractions
{
    public interface IAssignmentRepository
    {
        Task<Assignments> CreateAsync(Assignments assignments);
        Task UpdateAsync(Assignments assignments);
        Task DeleteAsync(Assignments assignments);
        Task<IEnumerable<Assignments>> GetAllAsync();
        Task<Assignments> GetByIdAsync(string id);
    }
}
