using MicroInventory.Category.Api.Domain.Entities;

namespace MicroInventory.Category.Api.Domain.Repositories.Abstractions
{
    public interface ICategoryRepository
    {
        Task<Categories> CreateAsync(Categories categories);
        Task UpdateAsync(Categories categories);
        Task DeleteAsync(Categories categories);
        Task<IEnumerable<Categories>> GetAllAsync();
        Task<Categories> GetByIdAsync(Guid id);

    }
}
