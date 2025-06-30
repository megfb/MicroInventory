using MicroInventory.Stock.Api.Domain.Entities;

namespace MicroInventory.Stock.Api.Domain.Repositories.Abstractions
{
    public interface IStockRepository
    {
        Task<Stocks> CreateAsync(Stocks stocks);
        Task UpdateAsync(Stocks stocks);
        Task DeleteAsync(Stocks stocks);
        Task<IEnumerable<Stocks>> GetAllAsync();
        Task<Stocks> GetByIdAsync(string id);
    }
}
