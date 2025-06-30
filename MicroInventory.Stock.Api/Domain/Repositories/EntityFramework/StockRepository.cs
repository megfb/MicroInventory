using MicroInventory.Shared.Common.Domain;
using MicroInventory.Stock.Api.Domain.Entities;
using MicroInventory.Stock.Api.Domain.Repositories.Abstractions;
using MicroInventory.Stock.Api.Domain.Repositories.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace MicroInventory.Stock.Api.Domain.Repositories.EntityFramework
{
    public class StockRepository(StockDbContext context, IUnitOfWork unitOfWork) : IStockRepository
    {
        public async Task<Stocks> CreateAsync(Stocks stocks)
        {
            await context.Stocks.AddAsync(stocks);
            await unitOfWork.SaveChangesAsync();
            return stocks;

        }

        public async Task DeleteAsync(Stocks stocks)
        {
            context.Stocks.Remove(stocks);
            await unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<Stocks>> GetAllAsync()
        {
            return await context.Stocks.ToListAsync();
        }

        public async Task<Stocks> GetByIdAsync(string id)
        {
            return await context.Stocks
                .FirstOrDefaultAsync(x => x.Id == id)
                ?? throw new KeyNotFoundException($"Category with ID {id} not found.");
        }

        public async Task UpdateAsync(Stocks stocks)
        {
            context.Stocks.Update(stocks);
            await unitOfWork.SaveChangesAsync();
        }
    }
}
