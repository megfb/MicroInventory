using MicroInventory.Shared.Common.Domain;
using MicroInventory.Stock.Api.Domain.Repositories.EntityFramework.DbContexts;

namespace MicroInventory.Stock.Api.Domain.Repositories
{
    public class UnitOfWork(StockDbContext context) : IUnitOfWork
    {
        public void Dispose()
        {
            context.Dispose();
        }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await context.SaveChangesAsync(cancellationToken);
        }
        public Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
